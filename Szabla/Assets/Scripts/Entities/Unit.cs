using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	public string Name;
	[SerializeField]
	private int health;
	public int Health { get { return health; } }
	[SerializeField]
	private int maxHealth;
	public int MaxHealth { get { return health; } }
}
