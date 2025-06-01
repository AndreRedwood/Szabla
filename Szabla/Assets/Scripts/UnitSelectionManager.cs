using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
	public static UnitSelectionManager Instance { get; set; }

	[SerializeField]
	private List<GameObject> allUnitsList = new List<GameObject>();
	[SerializeField]
	private List<GameObject> selectedUnitsList = new List<GameObject>();

	[SerializeField]
	private LayerMask clickable;
	[SerializeField]
	private LayerMask ground;
	[SerializeField]
	private GameObject groundMarker;

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

		if (Input.GetMouseButtonDown(1) && selectedUnitsList.Count > 0)
		{
			RaycastHit hit;
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
			{
				groundMarker.transform.position = hit.point + new Vector3(0f,0.1f,0f); //turn into offset!
				groundMarker.SetActive(true);
				foreach (GameObject unit in selectedUnitsList) 
				{
					unit.GetComponent<UnitMovement>().MoveOrder(hit.point); //add formation movement for group
				}
			}
		}
	}

	private void DeselctAll()
	{
		foreach(var unit in selectedUnitsList)
		{
			TriggerSelectionIndicator(unit, false);
		}
		groundMarker.SetActive(false);
		selectedUnitsList.Clear();

	}

	private void SelectUnit(GameObject unit, int mode = 0) //add unit display
	{
		if (mode == 0)
		{
			DeselctAll();
			selectedUnitsList.Add(unit);
			TriggerSelectionIndicator(unit, true);
		}
		else if (mode == 1)
		{
			if (!selectedUnitsList.Contains(unit))
			{
				selectedUnitsList.Add(unit);
				TriggerSelectionIndicator(unit, true);
			}
			else
			{
				TriggerSelectionIndicator(unit, false);
				selectedUnitsList.Remove(unit);
			}
		}
	}

	private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
	{
		unit.transform.GetChild(0).gameObject.SetActive(isVisible);
	}
}
