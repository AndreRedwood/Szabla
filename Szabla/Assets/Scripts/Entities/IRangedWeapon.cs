using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangedWeapon
{
    float EfficientRange {  get; }
	float MaximalRange { get; }

	float ReloadProgress { get; }

	void Shoot(Vector3 target, int skill, int shootingSkill);

	void Reload();
}
