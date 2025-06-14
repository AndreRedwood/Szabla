using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Weapon
{
	[SerializeField]
	protected string name;
	public string Name {  get { return name; } }
	[SerializeField] 
	protected int damage;
	public int DamageStat { get { return damage; } }
	public int Damage { get { return (int)(Random.Range(0.9f, 1.1f) * damage); } }
	//DO DEBUG!

	public Weapon(string name, int damage)
	{
		this.name = name;
		this.damage = damage;
	}
}
