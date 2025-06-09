using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMeleeWeapon
{
	float Reach { get; }

	void Attack(FlockAgent target, int meleeSkill);

	public IMeleeWeapon GetInterface { get; }
}
