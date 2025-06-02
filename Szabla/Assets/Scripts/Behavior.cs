using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Flock/Behavior/Test")]
public class Behavior : FlockBehavior
{
	Vector3 currentVelocity;
	public float agentSmoothTime = 0.5f;

	public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
	{

		//if (context.Count == 0)
		//{
		//	return Vector3.zero;
		//}
		//Vector3 cohesionMove = Vector3.zero;
		//foreach (Transform item in context) 
		//{
		//	cohesionMove += item.position;
		//}
		//cohesionMove /= context.Count;

		//cohesionMove -= agent.transform.position;
		//return cohesionMove;

		if (agent.IsMoving) 
		{
			Vector3 desiredVelocity = (agent.Destination - agent.transform.position).normalized;
			if(Vector3.Distance(agent.Destination, agent.transform.position) < 5f)
			{
				desiredVelocity *= (Vector3.Distance(agent.Destination, agent.transform.position) / 5f);
			}
			Vector3 steeredMove = desiredVelocity + agent.currentVelocity;

			return steeredMove;
		}
		return Vector3.zero;
	}
}
