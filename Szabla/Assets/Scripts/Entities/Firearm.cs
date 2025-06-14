using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Firearm : Weapon
{
	private float efficientRange;
	public float EfficientRange { get { return efficientRange; } }
	private float maximalRange;
	public float MaximalRange { get { return maximalRange; } }

	private float reloadTime;
	private float reloadProgress = 0f;
	public float ReloadProgress 
	{ 
		get 
		{
			if (reloadProgress >= reloadTime) return 1f;
			return reloadProgress / reloadTime;
		} 
	}
	public bool IsLoaded { get { return ReloadProgress == 1f ? true : false; } }

	private float[] dispersion;
	private float misfirePercentChance;

	//Bullet To do!

	public void Reload()
	{
		if(reloadProgress < reloadTime)
		{
			reloadProgress += Time.fixedDeltaTime;
		}
	}

	public void Shoot(Vector3 target, int skill, int shootingSkill)
	{
		throw new System.NotImplementedException();
	}

	public Firearm(string name, int damage, float[] range, float reloadTime, float[] dispersion, float misfirePercentChance) : 
		base(name, damage)
	{
		if(range.Length != 2) throw new System.ArgumentException("incorrect weapon range data");
		efficientRange = range[0];
		maximalRange = range[1];
		this.reloadTime = reloadTime;
		if(dispersion.Length != 2) throw new System.ArgumentException("incorrect weapon dispersion data");
		this.dispersion = dispersion;
		this.misfirePercentChance = misfirePercentChance;
	}
}
