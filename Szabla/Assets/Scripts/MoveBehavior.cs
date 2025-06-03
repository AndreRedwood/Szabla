using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveBehavior : ScriptableObject
{
	public const float speedFactor = 15f;
	public const float rotationFactor = 5f;
	public const float drag = 1f;

	public abstract SteeringData CalculateMove(FlockAgent agent, List<Transform> context);
}
