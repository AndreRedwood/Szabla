using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum MeleeWeapons
{
	handWeapon
}

[CreateAssetMenu(menuName = "Weapons/Melee Weapon")]
public class MeleeWeaponTemplate : WeaponTemplate
{
	[SerializeField]
	protected MeleeWeapons type;

	[SerializeField]
	protected float reach = 1f;
	[SerializeField]
	protected float baseAttackSpeed = 1f;


	public override Weapon Create()
	{
		switch (type) 
		{
			case MeleeWeapons.handWeapon: return new HandWeapon(name, damage, reach, baseAttackSpeed);
			default: throw new ArgumentException("type");
		}
	}
}
