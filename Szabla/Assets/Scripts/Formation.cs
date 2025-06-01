using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation
{
	public float UnitGap { get; }
	public float RankGap { get; }
	public float LooseFormation { get; }

	public Formation(float unitGap, float rankGap, float looseFormation = 0) 
	{
		UnitGap = unitGap;
		RankGap = rankGap;
		LooseFormation = looseFormation;
	}
}
