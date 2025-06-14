using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class HandWeapon : Weapon
{
	private float reach;
	public float Reach { get { return reach; } }
	private float baseAttackSpeed;
	public float BaseAttackSpeed { get { return baseAttackSpeed; } }

	public void Attack(FlockAgent target, int meleeSkill)
	{
		throw new System.NotImplementedException();
	}

	public HandWeapon(string name, int damage, float reach, float baseAttackSpeed) : base(name, damage)
	{
		this.reach = reach;
		this.baseAttackSpeed = baseAttackSpeed;
	}
}
