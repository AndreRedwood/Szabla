using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Flock/Behavior/Arrival")]
public class ArrivalBehavior : MoveBehavior
{
	[SerializeField]
	private float targetRadius = 0.1f;
	[SerializeField]
	private float slowRadius = 1f;

	public override SteeringData CalculateMove(FlockAgent agent, List<Transform> context)
	{
		Rigidbody body = agent.GetComponent<Rigidbody>();
		Vector3 target = agent.Destination;
		SteeringData steering = new SteeringData();

		Vector3 directiom = target - body.position;
		float distance = directiom.magnitude;

		if (distance < targetRadius)
		{
			body.velocity = Vector3.zero;
			return steering;
		}

		float targetSpeed;
		if (distance > slowRadius)
		{
			targetSpeed = speedFactor;
		}
		else
		{
			targetSpeed = speedFactor * (distance / slowRadius) * 1.5f;
		}

		Vector3 targetVelocity = directiom;
		targetVelocity.Normalize();
		targetVelocity *= targetSpeed;

		steering.linear = targetVelocity - body.velocity;
		if (steering.linear.magnitude > speedFactor)
		{
			steering.linear.Normalize();
			steering.linear *= speedFactor;
		}

		steering.angular = 0;

		return steering;
	}
}
