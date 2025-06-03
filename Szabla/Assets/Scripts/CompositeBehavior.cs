using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Flock/Behavior/CompositeBehavior")]
public class CompositeBehavior : MoveBehavior
{
	[SerializeField]
	private MoveBehavior[] behaviors;

	public override SteeringData CalculateMove(FlockAgent agent, List<Transform> context)
	{
		Vector3 acceleration = Vector3.zero;
		float rotation = 0;

		foreach(MoveBehavior behavior in behaviors) 
		{
			SteeringData steering = behavior.CalculateMove(agent, context);
			acceleration += steering.linear;
			rotation += steering.angular;
		}

		if(acceleration.magnitude > speedFactor)
		{
			acceleration.Normalize();
			acceleration *= speedFactor;
		}

		SteeringData result = new SteeringData();
		result.linear = acceleration;
		result.angular = rotation;

		return result;
	}
}
