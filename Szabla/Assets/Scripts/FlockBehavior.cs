using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehavior : ScriptableObject
{
	public virtual bool IsMoving { get; protected set; } = false;
	public virtual bool IsRotating { get; protected set; } = false;

	public abstract Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock);
}
