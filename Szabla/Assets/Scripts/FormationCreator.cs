using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FormationCreator : MonoBehaviour
{
	private const int rotationPrecision = 5;

	public static FormationCreator Instance { get; set; }

	public GameObject debugMarker;

	private void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	public List<Vector2> GenerateFormation(int unitCount, float rotation, Formation formation, int rankWidth = -1)
	{
		if(rankWidth < 0)
		{
			rankWidth = GetDefultRankWidth(unitCount);
		}

		int ranks = (unitCount / rankWidth);
		float rankLength = formation.UnitGap * rankWidth;
		List<Vector2> result = new List<Vector2>();

		if (ranks > 0)
		{
			Vector2[,] positions = new Vector2[ranks, rankWidth];

			positions[0, 0] = new Vector2(
				((rankLength / 2) - (formation.UnitGap / 2)) * (float)Math.Round(Mathf.Cos((rotation + 270f) * Mathf.Deg2Rad), rotationPrecision),
				((rankLength / 2) - (formation.UnitGap / 2)) * (float)Math.Round(Mathf.Sin((rotation + 270f) * Mathf.Deg2Rad), rotationPrecision)
				);

			for (int i = 1; i < rankWidth; i++)
			{
				positions[0, i] = new Vector2(
				positions[0, i - 1].x + formation.UnitGap *
				(float)Math.Round(Mathf.Cos((rotation + 90f) * Mathf.Deg2Rad), rotationPrecision),
				positions[0, i - 1].y + formation.UnitGap *
				(float)Math.Round(Mathf.Sin((rotation + 90f) * Mathf.Deg2Rad), rotationPrecision)
				);
			}

			for (int r = 1; r < ranks; r++)
			{
				for (int i = 0; i < rankWidth; i++)
				{
					positions[r, i] = new Vector2(
					positions[r - 1, i].x - formation.RankGap *
					(float)Math.Round(Mathf.Cos((rotation + 180f) * Mathf.Deg2Rad), rotationPrecision),
					positions[r - 1, i].y - formation.RankGap *
					(float)Math.Round(Mathf.Sin((rotation + 180f) * Mathf.Deg2Rad), rotationPrecision)
					);
				}
			}

			foreach (Vector2 position in positions)
			{
				Vector2 positionPass = position;
				if(formation.LooseFormation > 0)
				{
					positionPass.x += UnityEngine.Random.Range(-formation.LooseFormation, formation.LooseFormation);
					positionPass.y += UnityEngine.Random.Range(-formation.LooseFormation, formation.LooseFormation);
				}
				result.Add(positionPass);
			}
		}
		Vector2[] lastRankPositions = null;
		if (unitCount % rankWidth != 0)
		{
			lastRankPositions = new Vector2[unitCount % rankWidth];
			Vector2 lastRankStart = new Vector2(
				(ranks) * -formation.RankGap * (float)Math.Round(Mathf.Cos((rotation + 180f) * Mathf.Deg2Rad), rotationPrecision),
				(ranks) * -formation.RankGap * (float)Math.Round(Mathf.Sin((rotation + 180f) * Mathf.Deg2Rad), rotationPrecision)
				);
			float lastRankLength = lastRankPositions.Length * formation.UnitGap;
			lastRankStart += new Vector2(
				((lastRankLength / 2) - (formation.UnitGap / 2)) * (float)Math.Round(Mathf.Cos((rotation + 270f) * Mathf.Deg2Rad), rotationPrecision),
				((lastRankLength / 2) - (formation.UnitGap / 2)) * (float)Math.Round(Mathf.Sin((rotation + 270f) * Mathf.Deg2Rad), rotationPrecision)
				);
			lastRankPositions[0] = new Vector2( lastRankStart.x, lastRankStart.y );
			for (int i = 1; i < lastRankPositions.Length; i++)
			{
				lastRankPositions[i] = new Vector2(
				lastRankPositions[i - 1].x + formation.UnitGap *
				(float)Math.Round(Mathf.Cos((rotation + 90f) * Mathf.Deg2Rad), rotationPrecision),
				lastRankPositions[i - 1].y + formation.UnitGap *
				(float)Math.Round(Mathf.Sin((rotation + 90f) * Mathf.Deg2Rad), rotationPrecision)
				);
			}
		}
		if(lastRankPositions != null)
		{
			foreach (Vector2 position in lastRankPositions) 
			{
				Vector2 positionPass = position;
				if (formation.LooseFormation > 0)
				{
					positionPass.x += UnityEngine.Random.Range(-formation.LooseFormation, formation.LooseFormation);
					positionPass.y += UnityEngine.Random.Range(-formation.LooseFormation, formation.LooseFormation);
				}
				result.Add(positionPass);
			}
		}
		return result;
	}

	private int GetDefultRankWidth(int unitCount)
	{
		switch (unitCount)
		{
			case < 4: return unitCount;
			case < 13: return 4;
			case < 21: return 5;
			default: return 8;
		}
	}
}
