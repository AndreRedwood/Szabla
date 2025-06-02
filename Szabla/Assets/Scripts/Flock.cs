using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
	[SerializeField]
	List<FlockAgent> agents = new List<FlockAgent>();
	public FlockBehavior behavior;

	[Range(1f, 100f)]
	public float driveFactor = 10f;
	[Range(1f, 100f)]
	public float maxSpeed = 5f;
	[Range(1f, 10f)]
	public float neighborRadius = 1.5f;
	[Range(0f, 1f)]
	public float avoidanceRadiusMultiplayer = 0.5f;

	float squareMaxSpeed;
	float squareNeighborRadius;
	float squareAvoidanceRadius;
	public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    // Start is called before the first frame update
    void Start()
    {
		squareMaxSpeed = maxSpeed * maxSpeed;
		squareNeighborRadius = neighborRadius * neighborRadius;
		squareAvoidanceRadius = squareAvoidanceRadius * avoidanceRadiusMultiplayer * avoidanceRadiusMultiplayer;	
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if(agents.Count == 0) 
		{
			List<GameObject> agentObjects = UnitSelectionManager.Instance.AllUnitsList;
			foreach (GameObject agent in agentObjects)
			{
				agent.GetComponent<FlockAgent>().flock = this;
				agents.Add(agent.GetComponent<FlockAgent>());
			}
		}

        foreach(FlockAgent agent in agents)
		{
			List<Transform> contex = GetNearbyObjects(agent);
			Vector3 move = behavior.CalculateMove(agent, contex, this);
			move *= driveFactor;

			if (move.sqrMagnitude > squareMaxSpeed)
			{
				move = move.normalized * maxSpeed;
			}
			agent.Move(move);
		}
    }

	List<Transform> GetNearbyObjects(FlockAgent agent)
	{
		List<Transform> context = new List<Transform>();
		Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
		foreach (Collider collider in contextColliders)
		{
			if(collider != agent.AgentCollider)
			{
				context.Add(collider.transform);
			}
		}
		return context;
	}
}
