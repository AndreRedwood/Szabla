using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionBox : MonoBehaviour
{
	Camera cam;

	[SerializeField]
	private RectTransform boxVisual;
	private Rect selectionBox;

	private Vector2 startPosition;
	private Vector2 endPosition;

    private void Start()
    {
        cam = Camera.main;
		startPosition = Vector2.zero;
		endPosition = Vector2.zero;
		DrawVisual();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
		{
			startPosition = Input.mousePosition;
			selectionBox = new Rect();
		}

		if(Input.GetMouseButton(0))
		{
			if(boxVisual.rect.width > 0 || boxVisual.rect.height > 0)
			{
				UnitSelectionManager.Instance.DeselctAll();
				SelectUnits();
			}

			endPosition = Input.mousePosition;
			DrawVisual();
			DrawSelection();
		}

		if(Input.GetMouseButtonUp(0))
		{
			SelectUnits();
			startPosition = Vector2.zero;
			endPosition = Vector2.zero;
			DrawVisual();
		}
    }

	private void DrawVisual()
	{
		Vector2 boxCenter = (startPosition + endPosition) / 2;
		boxVisual.position = boxCenter;
		Vector2 boxSize = new Vector2(
			Mathf.Abs(startPosition.x - endPosition.x), 
			Mathf.Abs(startPosition.y - endPosition.y)
			);
		boxVisual.sizeDelta = boxSize;
	}

	private void DrawSelection()
	{
		if(Input.mousePosition.x < startPosition.x)
		{
			selectionBox.xMin = Input.mousePosition.x;
			selectionBox.xMax = startPosition.x;
		}
		else
		{
			selectionBox.xMin = startPosition.x;
			selectionBox.xMax = Input.mousePosition.x;
		}

		if (Input.mousePosition.y < startPosition.y)
		{
			selectionBox.yMin = Input.mousePosition.y;
			selectionBox.yMax = startPosition.y;
		}
		else
		{
			selectionBox.yMin = startPosition.y;
			selectionBox.yMax = Input.mousePosition.y;
		}
	}

	private void SelectUnits()
	{
		foreach(var unit in UnitSelectionManager.Instance.AllUnitsList) 
		{
			if(selectionBox.Contains(cam.WorldToScreenPoint(unit.transform.position)))
			{
				UnitSelectionManager.Instance.SelectUnit(unit, 2);
			}
		}
	}
}
