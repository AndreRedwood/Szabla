using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Ranged/Firearm")]
public class Firearm : Weapon, IRangedWeapon
{
	[SerializeField]
	private float efficientRange;
	public float EfficientRange { get { return efficientRange; } }
	[SerializeField]
	private float maximalRange;
	public float MaximalRange { get { return maximalRange; } }

	[SerializeField]
	private float reloadProgress = 0f;
	public float ReloadProgress 
	{ 
		get 
		{
			if (reloadProgress >= loadingTime) return 1f;
			return reloadProgress / loadingTime;
		} 
	}
	public bool IsLoaded { get { return ReloadProgress == 1f ? true : false; } }
	[SerializeField]
	private float loadingTime;

	[SerializeField]
	private float[] dispersion = new float[2];
	[SerializeField] [Range (0f, 100f)]
	private float misfirePercentChance;

	[SerializeField]
	private GameObject bulletPrefab;

	public void Reload()
	{
		if(reloadProgress < loadingTime)
		{
			reloadProgress += Time.fixedDeltaTime;
		}
	}

	public void Shoot(Vector3 target, int skill, int shootingSkill)
	{
		throw new System.NotImplementedException();
	}

	public IRangedWeapon GetInterface { get { return this; } }
}
