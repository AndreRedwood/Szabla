using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponTemplate : ScriptableObject
{
	[SerializeField]
	protected new string name;
	[SerializeField]
	protected int damage;
	[SerializeField]
	protected Color image;

	public abstract Weapon Create();
}
