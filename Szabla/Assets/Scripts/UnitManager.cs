using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private void Start()
    {
		UnitSelectionManager.Instance.AllUnitsList.Add(gameObject);
    }

    private void OnDestroy()
    {
		UnitSelectionManager.Instance.AllUnitsList.Remove(gameObject);
    }
}
