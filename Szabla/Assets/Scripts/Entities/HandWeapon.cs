using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Melee/HandWeapon")]
public class HandWeapon : Weapon, IMeleeWeapon
{
	private float reach;
	public float Reach { get { return reach; } }

	public void Attack(FlockAgent target, int meleeSkill)
	{
		throw new System.NotImplementedException();
	}

	public IMeleeWeapon GetInterface { get { return this; } }
}
