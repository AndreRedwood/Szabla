using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/AlignWithMovement")]
public class AlignWithMovementBehavior : MoveBehavior
{
	public override SteeringData CalculateMove(FlockAgent agent, List<Transform> context)
	{
		Rigidbody body = agent.GetComponent<Rigidbody>();
		SteeringData steering = new SteeringData();

		if (body.velocity.magnitude == 0)
		{
			return steering;
		}

		float angle = Mathf.Atan2(body.velocity.x, body.velocity.z) * Mathf.Rad2Deg;
		steering.angular = Mathf.LerpAngle(body.rotation.eulerAngles.y, angle, rotationFactor * Time.fixedDeltaTime);

		steering.linear = Vector3.zero;
		
		return steering;
	}
}
