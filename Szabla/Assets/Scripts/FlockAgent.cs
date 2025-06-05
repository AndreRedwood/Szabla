using JetBrains.Rider.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
	private Collider agentCollider;
	public Collider AgentCollider { get { return agentCollider; } }
	private Rigidbody body;

	[Header("UnitStatistics")]
	public string unitName;
	[SerializeField]
	private int maxHealth;
	[SerializeField] 
	private int health;
	public int[] Health { get { return new int[2] {health, maxHealth}; } }
	[SerializeField]
	private float moveSpeed = 1f;

	[SerializeField]
	private int skill = 4;
	public int Skill { get { return skill; } }
	[SerializeField]
	private int morale = 5;
	public int Morale { get { return morale; } }

	[Header("Equipnment")]
	[SerializeField]
	private List<Weapon> weapons;
	public List<Weapon> Weapons { get {  return weapons; } }

	[Header("AI Properties")]
	[SerializeField]
	private MoveBehavior moveBehavior = null;
	public MoveBehavior Behavior { get { return moveBehavior; } set { moveBehavior = value; } }
	[SerializeField]
	private Vector3 destination;
	public Vector3 Destination { get { return destination; } set { destination = value; } }


    void Start()
    {
        agentCollider = GetComponent<Collider>();
		body = GetComponent<Rigidbody>();
		Destination = body.position;
    }

	private void FixedUpdate()
	{
		if (moveBehavior != null)
		{
			SteeringData steering = moveBehavior.CalculateMove(this, GetNearbyObjects(this));

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

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, Destination);
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
