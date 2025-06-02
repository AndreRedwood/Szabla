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

	public FlockBehavior behavior = null;
	public Flock flock;

	public bool IsMoving = false;
	public Vector3 Destination { get; set; }
	public float Angle;
	public Vector3 currentVelocity;

	public float StoppingDistance = 0.15f;
	public float RotationSpeed = 100f;

    void Start()
    {
        agentCollider = GetComponent<Collider>();
		body = GetComponent<Rigidbody>();
    }

	private void FixedUpdate()
	{
		if (behavior != null)
		{
			//Move(behavior.CalculateMove(this, GetNearbyObjects(this), flock));
			Move(Destination);
		}
	}

	public void Move(Vector3 velocity)
	{
		//if (Vector3.Distance(transform.position, destination) < StoppingDistance)
		//{
		//	behavior = null;
		//	velocity = Vector3.zero;
		//	return;
		//}
		//currentVelocity = velocity;
		//body.position += velocity * Time.deltaTime;
		//if (velocity != Vector3.zero && body.rotation != Quaternion.LookRotation(velocity, Vector3.up))
		//{
		//	Quaternion direction2 = Quaternion.LookRotation(velocity, Vector3.up);
		//	body.MoveRotation(Quaternion.RotateTowards(body.rotation, direction2, Time.deltaTime * RotationSpeed));
		//	velocity = Vector3.zero;
		//	return;
		//}
		//if (velocity != Vector3.zero)
		//{
		//	Quaternion direction2 = Quaternion.LookRotation(velocity, Vector3.up);
		//	body.MoveRotation(Quaternion.RotateTowards(body.rotation, direction2, Time.deltaTime * RotationSpeed));
		//	velocity = Vector3.zero;
		//}

		if(behavior == null)
		{
			return;
		}

		Vector3 linear = Vector3.zero;
		float angular = 0f;

		body.drag = 1f;

		Vector3 acceleration = Vector3.zero;
		float rotation = 0;

		Vector3 target = Destination;
		//Vector3 target = UnitSelectionManager.Instance.groundMarkers[0].transform.position;
		float targetRadius = 0.1f;
		float slowRadius = 1f;

		Vector3 directiom = target - body.position;
		float distance = directiom.magnitude;
		if(distance < targetRadius)
		{
			body.velocity = Vector3.zero;
		}
		float targetSpeed;
		if(distance > slowRadius)
		{
			targetSpeed = 5f;
		}
		else
		{
			targetSpeed = 10f * (distance / slowRadius);
		}
		Vector3 targetVelocity = directiom;
		targetVelocity.Normalize();
		targetVelocity *= targetSpeed;

		linear = targetVelocity - body.velocity;
		if(linear.magnitude > 5f)
		{
			linear.Normalize();
			linear *= 5f;
		}
		angular = 0;

		acceleration += linear;
		rotation += angular;

		if(body.velocity.magnitude != 0)
		{
			float angle = Mathf.Atan2(body.velocity.x, body.velocity.z) * Mathf.Rad2Deg;
			angular = Mathf.LerpAngle(transform.rotation.eulerAngles.y, angle, 3f * Time.fixedDeltaTime);
			rotation += angular;
		}

		if(acceleration.magnitude > 5f)
		{
			acceleration.Normalize();
			acceleration *= 5f;
		}
		body.AddForce(acceleration*5f);
		if(rotation !=0) 
		{
			body.rotation = Quaternion.Euler(0, rotation, 0);
		}

		//Quaternion direction = Quaternion.LookRotation(velocity, Vector3.up);
		//body.rotation = Quaternion.RotateTowards(body.rotation, direction, Time.deltaTime * RotationSpeed);
	}

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
