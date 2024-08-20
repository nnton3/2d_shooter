using Assets.Scripts.Game.Level;
using Assets.Scripts.ScriptableObjects.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
	[CreateAssetMenu(fileName = "LevelSettings", menuName = "Game/LevelSettings")]
	public class LevelSettings : ScriptableObject
	{
		public Vector2 SpawnDelayRange;
		public Vector2Int EnemiesCountRange;
		public List<UnitSpawnData> EnemyTypesList;

		public LevelData ToLevelData()
		{
			var enemiesCount = Random.Range(EnemiesCountRange.x, EnemiesCountRange.y + 1);
			var enemiesSpawnData = new List<UnitSpawnData>();
			var enemiesSpawnDelays = new List<float>();
			for (int i = 0; i < enemiesCount; i++)
			{
				enemiesSpawnData.Add(EnemyTypesList[Random.Range(0, EnemyTypesList.Count)]);
				enemiesSpawnDelays.Add(Random.Range(SpawnDelayRange.x, SpawnDelayRange.y));
			}
			return new LevelData
			{
				UnitsSpawnData = enemiesSpawnData,
				SpawnDelays = enemiesSpawnDelays
			};
		}
	}
}
