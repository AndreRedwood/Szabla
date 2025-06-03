using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Separation")]
public class SeparationBehavior : MoveBehavior
{
	[SerializeField]
	private float threshold = 1f;
	[SerializeField]
	private float decayCoefficient = -25f;

	public override SteeringData CalculateMove(FlockAgent agent, List<Transform> context)
	{
		Rigidbody body = agent.GetComponent<Rigidbody>();
		SteeringData steering = new SteeringData();

		foreach (Transform entity in context)
		{
			Vector3 direction = entity.position - body.position;
			float distance = direction.magnitude;
			if (distance < threshold)
			{
				float strength = Mathf.Min(decayCoefficient / (distance * distance), speedFactor);
				direction.Normalize();
				steering.linear += strength * direction;
			}
		}
		return steering;
	}
}
