using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject
{
	[SerializeField]
	protected new string name;
	public string Name {  get { return name; } protected set { name = value; } }
	[SerializeField] 
	protected int damage;
	public int Damage { get { return damage; } protected set { damage = value; } }
}
