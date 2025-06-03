using JetBrains.Rider.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
	private Collider agentCollider;
	public Collider AgentCollider { get { return agentCollider; } }
	private Rigidbody body;

	[SerializeField]
	private MoveBehavior behavior = null;
	public MoveBehavior Behavior { get { return behavior; } set { behavior = value; } }
	public Vector3 Destination { get; set; }

	[SerializeField]
	private float moveSpeed = 1f;

    void Start()
    {
        agentCollider = GetComponent<Collider>();
		body = GetComponent<Rigidbody>();
    }

	private void FixedUpdate()
	{
		if (behavior != null)
		{
			SteeringData steering = behavior.CalculateMove(this, GetNearbyObjects(this));

			if (steering.linear.magnitude > MoveBehavior.speedFactor)
			{
				steering.linear.Normalize();
				steering.linear *= MoveBehavior.speedFactor;
			}

			steering.linear *= moveSpeed;
			body.AddForce(steering.linear);
			if (steering.angular != 0)
			{
				body.rotation = Quaternion.Euler(0, steering.angular, 0);
			}
		}
	}

	//add variable radius
	List<Transform> GetNearbyObjects(FlockAgent agent)
	{
		List<Transform> context = new List<Transform>();
		Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, 3f);
		foreach (Collider collider in contextColliders)
		{
			if (collider != agent.AgentCollider)
			{
				context.Add(collider.transform);
			}
		}
		return context;
	}
}
