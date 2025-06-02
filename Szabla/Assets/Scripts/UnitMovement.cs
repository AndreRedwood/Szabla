using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
	private Camera cam;
	private NavMeshAgent agent;
	[SerializeField]
	private LayerMask ground;
	
	private void Start()
    {
		cam = Camera.main;
		agent = GetComponent<NavMeshAgent>();
    }

    public void MoveOrder(Vector3 destination)
    {
		//agent.SetDestination(destination);
		//rework to steering behaviour
    }
}
