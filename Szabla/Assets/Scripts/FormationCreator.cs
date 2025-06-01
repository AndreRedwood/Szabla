using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FormationCreator : MonoBehaviour
{
	public static FormationCreator Instance { get; set; }

	public GameObject prefabTest;

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

	public List<Vector2> GenerateFormation(int unitCount, int rankWidth, float rotation, Formation formation)
	{
		int ranks = unitCount / rankWidth;

		float rankLength = formation.UnitGap * rankWidth;

		Vector2[,] positions = new Vector2[ranks, rankWidth];

		positions[0,0] = new Vector2(
			((rankLength / 2) - (formation.UnitGap / 2)) * (float)Math.Round(Mathf.Cos((rotation + 270f) * Mathf.Deg2Rad), 5),
			((rankLength / 2) - (formation.UnitGap / 2)) * (float)Math.Round(Mathf.Sin((rotation + 270f) * Mathf.Deg2Rad), 5)
			);
		Instantiate(prefabTest, new Vector3(positions[0,0].x, 0.2f, positions[0, 0].y), Quaternion.identity);
		for(int i = 1; i < rankWidth; i++)
		{
			positions[0, i] = new Vector2(
			positions[0, i - 1].x + formation.UnitGap * 
			(float)Math.Round(Mathf.Cos((rotation + 90f) * Mathf.Deg2Rad), 5),
			positions[0, i - 1].y + formation.UnitGap * 
			(float)Math.Round(Mathf.Sin((rotation + 90f) * Mathf.Deg2Rad), 5)
			);
			Instantiate(prefabTest, new Vector3(positions[0, i].x, 0.2f, positions[0, i].y), Quaternion.identity);
		}
		for(int r = 1; r < unitCount / rankWidth; r++)
		{
			for (int i = 0; i < rankWidth; i++)
			{
				positions[r, i] = new Vector2(
				positions[r - 1, i].x + formation.RankGap *
				(float)Math.Round(Mathf.Cos((rotation + 180f) * Mathf.Deg2Rad), 5),
				positions[r - 1, i].y + formation.RankGap *
				(float)Math.Round(Mathf.Sin((rotation + 180f) * Mathf.Deg2Rad), 5)
				);
				Instantiate(prefabTest, new Vector3(positions[r, i].x, 0.2f, positions[r, i].y), Quaternion.identity);
			}
		}
		if(unitCount % rankWidth != 0)
		{
			Vector2[] lastRankPositions = new Vector2[unitCount % rankWidth];
			Vector2 lastRankStart = new Vector2(
				(ranks) * formation.RankGap * (float)Math.Round(Mathf.Cos((rotation + 180f) * Mathf.Deg2Rad), 5),
				(ranks) * formation.RankGap * (float)Math.Round(Mathf.Sin((rotation + 180f) * Mathf.Deg2Rad), 5)
				);
			float lastRankLength = lastRankPositions.Length * formation.UnitGap;
			lastRankStart += new Vector2(
				((lastRankLength / 2) - (formation.UnitGap / 2)) * (float)Math.Round(Mathf.Cos((rotation + 270f) * Mathf.Deg2Rad), 5),
				((lastRankLength / 2) - (formation.UnitGap / 2)) * (float)Math.Round(Mathf.Sin((rotation + 270f) * Mathf.Deg2Rad), 5)
				);
			lastRankPositions[0] = new Vector2( lastRankStart.x, lastRankStart.y );
			Instantiate(prefabTest, new Vector3(lastRankPositions[0].x, 0.2f, lastRankPositions[0].y), Quaternion.identity);
			for (int i = 1; i < lastRankPositions.Length; i++)
			{
				lastRankPositions[i] = new Vector2(
				lastRankPositions[i - 1].x + formation.UnitGap *
				(float)Math.Round(Mathf.Cos((rotation + 90f) * Mathf.Deg2Rad), 5),
				lastRankPositions[i - 1].y + formation.UnitGap *
				(float)Math.Round(Mathf.Sin((rotation + 90f) * Mathf.Deg2Rad), 5)
				);
				Instantiate(prefabTest, new Vector3(lastRankPositions[i].x, 0.2f, lastRankPositions[i].y), Quaternion.identity);
			}
		}

		return null;
	}
}
