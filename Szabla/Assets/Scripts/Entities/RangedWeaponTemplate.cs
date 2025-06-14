using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[System.Serializable]
public enum RangedWeapons
{
	firearm,
	bow
}

[CreateAssetMenu(menuName = "Weapons/Ranged Weapon")]
public class RangedWeaponTemplate : WeaponTemplate
{
	[SerializeField]
	protected RangedWeapons type;

	[SerializeField]
	protected float efficientRange;
	[SerializeField]
	protected float maximalRange;

	[SerializeField]
	protected float reloadTime;

	[Header("Firearm Specific")]
	[SerializeField]
	protected float[] dispersion = new float[2];
	[SerializeField]
	[Range(0f, 100f)]
	protected float misfirePercentChance;

	public override Weapon Create()
	{
		switch (type) 
		{
			case RangedWeapons.firearm: return new Firearm(name, damage, new float[] { efficientRange, maximalRange}, reloadTime, dispersion, misfirePercentChance); 
			case RangedWeapons.bow: throw new System.NotImplementedException();
			default: throw new System.ArgumentException("type");
		}
	}
}
