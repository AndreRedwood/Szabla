using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

	private List<GameObject> groundMarkers = new List<GameObject>();

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
				foreach (GameObject unit in SelectedUnitsList) 
				{
					unit.GetComponent<UnitMovement>().MoveOrder(hit.point); //add formation movement for group
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
