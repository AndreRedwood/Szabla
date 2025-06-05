using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using UnityEditor;
using Unity.IO.LowLevel.Unsafe;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitSelectionManager : MonoBehaviour
{
	public static UnitSelectionManager Instance { get; set; }

	private List<GameObject> allUnitsList = new List<GameObject>();
	public List<GameObject> AllUnitsList { get { return allUnitsList; } }
	[SerializeField]
	private List<GameObject> selectedUnitsList = new List<GameObject>();
	public List <GameObject> SelectedUnitsList { get { return selectedUnitsList; } }

	[SerializeField]
	private LayerMask clickable;
	[SerializeField]
	private LayerMask ground;
	[SerializeField]
	private GameObject groundMarkerPrefab;
	[SerializeField]
	private const float GroundMarkerOffset = 0.1f;

	public List<GameObject> groundMarkers = new List<GameObject>();

	[SerializeField]
	private float defultFormationUnitGap;
	[SerializeField]
	private float defultFormationRankGap;
	[SerializeField]
	private float defultLooseFormationRandomness;

	[SerializeField]
	private bool isFrontWidhtSet = false;
	[SerializeField]
	private bool isFormationLoose = false;

	[SerializeField] [Range(2, 10)]
	private int frontWidth;

	private Formation defultFormation;
	private Formation defulfFormationLoose;

	public MoveBehavior moveBeh;

	private Camera cam;
	private SingleUnitSelectWindow singleSelectWindow;

	private void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	private void Start()
	{
		cam = Camera.main;
		defultFormation = new Formation(
			defultFormationUnitGap,
			defultFormationRankGap
			);
		defulfFormationLoose = new Formation(
			defultFormation.UnitGap * 2, 
			defultFormation.RankGap * 2, 
			defultLooseFormationRandomness);
		singleSelectWindow = GetComponent<SingleUnitSelectWindow>();
	}

	private void Update()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			if (Input.GetMouseButtonDown(0))
			{
				RaycastHit hit;
				Ray ray = cam.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
				{
					if (Input.GetKey(KeyCode.LeftShift))
					{
						SelectUnit(hit.collider.gameObject, 1);
					}
					else
					{
						SelectUnit(hit.collider.gameObject);
					}
				}
				else
				{
					if (!Input.GetKey(KeyCode.LeftShift))
					{
						DeselctAll();
					}
				}
			}

			if (Input.GetMouseButtonDown(1) && SelectedUnitsList.Count > 0)
			{
				RaycastHit hit;
				Ray ray = cam.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
				{
					foreach (var marker in groundMarkers)
					{
						Destroy(marker.gameObject);
					}
					groundMarkers.Clear();

					GameObject groundMarker = Instantiate(groundMarkerPrefab);
					groundMarkers.Add(groundMarker);
					groundMarker.transform.position = hit.point + new Vector3(0f, GroundMarkerOffset, 0f);
					groundMarker.SetActive(true);

					if (SelectedUnitsList.Count > 1)
					{
						Vector3 centroid = GetCentroid(selectedUnitsList);
						float angle = (float)Math.Round(Mathf.Atan2(
							centroid.z - hit.point.z,
							centroid.x - hit.point.x)
							* Mathf.Rad2Deg, 5
							);

						List<Vector2> positions = FormationCreator.Instance.GenerateFormation(
							selectedUnitsList.Count, angle,
							isFormationLoose ? defulfFormationLoose : defultFormation,
							isFrontWidhtSet ? frontWidth : -1);

						List<FlockAgent> unitsQueue = new List<FlockAgent>();
						foreach (GameObject unit in selectedUnitsList)
						{
							unitsQueue.Add(unit.GetComponent<FlockAgent>());
						}

						foreach (Vector2 position in positions)
						{
							Vector2 hitPosition = ParsePosition(hit.point);

							FlockAgent nearestUnit = unitsQueue[0];
							Vector2 nearestPosition = nearestUnit.transform.position;
							float nearestDistance = Vector2.Distance(nearestPosition, position + hitPosition);

							foreach (FlockAgent unit in unitsQueue)
							{
								Vector2 unitPosition = ParsePosition(unit.transform.position);
								float distance = Vector2.Distance(unitPosition, position + hitPosition);

								if (distance < nearestDistance)
								{
									nearestUnit = unit;
									nearestPosition = unitPosition;
									nearestDistance = distance;
								}
							}

							nearestUnit.Destination = new Vector3(
								hit.point.x + position.x,
								1f,
								hit.point.z + position.y
								);
							nearestUnit.Behavior = moveBeh;
							unitsQueue.Remove(nearestUnit);
						}
					}
					else
					{
						//can be done without foreach most likely
						foreach (GameObject unit in SelectedUnitsList)
						{
							unit.GetComponent<FlockAgent>().Destination = new Vector3(
								hit.point.x,
								1f,
								hit.point.z
								);
							unit.GetComponent<FlockAgent>().Behavior = moveBeh;
						}
					}

				}
			}
		}
	}

	public void DeselctAll()
	{
		foreach(var unit in SelectedUnitsList)
		{
			TriggerSelectionIndicator(unit, false);
		}
		foreach(var marker in groundMarkers)
		{
			Destroy(marker.gameObject);
		}
		groundMarkers.Clear();
		SelectedUnitsList.Clear();
		singleSelectWindow.Deselect();
	}

	public void SelectUnit(GameObject unit, int mode = 0) //add unit display
	{
		if (mode == 0)
		{
			DeselctAll();
			SelectedUnitsList.Add(unit);
			TriggerSelectionIndicator(unit, true);
			singleSelectWindow.Select(unit.GetComponent<FlockAgent>());
		}
		else if (mode == 1)
		{
			if (!SelectedUnitsList.Contains(unit))
			{
				SelectedUnitsList.Add(unit);
				TriggerSelectionIndicator(unit, true);
			}
			else
			{
				TriggerSelectionIndicator(unit, false);
				SelectedUnitsList.Remove(unit);
			}
		}
		else if (mode == 2)
		{
			if (!SelectedUnitsList.Contains(unit)) 
			{
				SelectedUnitsList.Add(unit);
				TriggerSelectionIndicator(unit, true);
			}
		}
	}

	[SerializeField]
	private TextMeshProUGUI LooseFormationButton;

	public void SwitchFormation()
	{
		isFormationLoose = !isFormationLoose;
		LooseFormationButton.text = isFormationLoose ? "L" : "T";
	}

	[SerializeField]
	private TextMeshProUGUI ManualWidthLabel;
	[SerializeField]
	private Button[] ManualWidthButtons;

	public void SwitchManualFront()
	{
		isFrontWidhtSet = !isFrontWidhtSet;
		foreach(Button button in ManualWidthButtons)
		{
			button.interactable = isFrontWidhtSet;
		}
	}

	public void ManualFrontModify(int value)
	{
		frontWidth += value;
		if (frontWidth < 3) frontWidth = 2;
		else if (frontWidth > 10) frontWidth = 10;
		ManualWidthLabel.text = frontWidth.ToString();
	}

	private Vector3 GetCentroid(List<GameObject> entities)
	{
		Vector3 result = Vector3.zero;
		foreach (GameObject entity in entities)
		{
			result += entity.transform.position;
		}
		result /= SelectedUnitsList.Count;
		return result;
	}

	//change name + move somwhere else
	private Vector2 ParsePosition(Vector3 coordinates)
	{
		return new Vector2(coordinates.x, coordinates.z);
	}

	private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
	{
		unit.transform.GetChild(0).gameObject.SetActive(isVisible);
	}
}
