using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using UnityEditor;

public class UnitSelectionManager : MonoBehaviour
{
	public static UnitSelectionManager Instance { get; set; }

	private List<GameObject> allUnitsList = new List<GameObject>();
	public List<GameObject> AllUnitsList { get { return allUnitsList; } }
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

	[SerializeField]
	private int frontWidth;

	private Formation defultFormation;
	private Formation defulfFormationLoose;

	public Behavior moveBeh;

	private Camera cam;

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
			defultFormation.UnitGap, 
			defultFormation.RankGap, 
			defultLooseFormationRandomness);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
			{
				if(Input.GetKey(KeyCode.LeftShift))
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
					Vector3 centroid = Vector3.zero;
					foreach (GameObject unit in SelectedUnitsList)
					{
						centroid += unit.transform.position;
					}
					centroid /= SelectedUnitsList.Count;
					float angle = (float)Math.Round(Mathf.Atan2(
						centroid.z - hit.point.z,
						centroid.x - hit.point.x)
						* Mathf.Rad2Deg, 5
						);
					List<Vector2> positions = FormationCreator.Instance.GenerateFormation(
						selectedUnitsList.Count, angle, 
						isFormationLoose ? defulfFormationLoose : defultFormation, 
						isFrontWidhtSet ? frontWidth : -1);
					for (int i = 0; i < SelectedUnitsList.Count; i++)
					{
						//SelectedUnitsList[i].GetComponent<UnitMovement>().MoveOrder(new Vector3(
						//	hit.point.x + positions[i].x,
						//	hit.point.y,
						//	hit.point.z + positions[i].y
						//	));
						selectedUnitsList[i].GetComponent<FlockAgent>().Destination = new Vector3(
							hit.point.x + positions[i].x,
							hit.point.y,
							hit.point.z + positions[i].y
							);
						selectedUnitsList[i].GetComponent<FlockAgent>().behavior = moveBeh;
						//add formation movement for group
					}
				}
				else
				{
					foreach (GameObject unit in SelectedUnitsList)
					{
						float angle = (float)Math.Round(Mathf.Atan2(
						unit.transform.position.z - hit.point.z,
						unit.transform.position.x - hit.point.x)
						* Mathf.Rad2Deg, 5
						);
						//Debug.Log(angle);
						//unit.GetComponent<FlockAgent>().Destination = hit.point;
						unit.GetComponent<FlockAgent>().Destination = hit.point;
						unit.GetComponent<FlockAgent>().behavior = moveBeh;
						//unit.GetComponent<UnitMovement>().MoveOrder(hit.point); //add formation movement for group
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

	}

	public void SelectUnit(GameObject unit, int mode = 0) //add unit display
	{
		if (mode == 0)
		{
			DeselctAll();
			SelectedUnitsList.Add(unit);
			TriggerSelectionIndicator(unit, true);
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

	private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
	{
		unit.transform.GetChild(0).gameObject.SetActive(isVisible);
	}
}
