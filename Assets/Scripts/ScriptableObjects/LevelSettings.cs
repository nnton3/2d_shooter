using Assets.Scripts.Game.Level;
using Assets.Scripts.Helpers;
using Assets.Scripts.ScriptableObjects.Units;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.ScriptableObjects
{
	[CreateAssetMenu(fileName = "LevelSettings", menuName = "Game/LevelSettings")]
	public class LevelSettings : ScriptableObject
	{
		public FloatRangeValue SpawnDelayRange;
		public IntRangeValue EnemiesCountRange;
		public UnitSpawnData[] EnemyTypesList;

		public LevelData ToLevelData()
		{
			var enemiesCount = EnemiesCountRange.GetValue();
			var enemiesSpawnData = new UnitSpawnData[enemiesCount];
			var enemiesSpawnDelays = new float[enemiesCount];
			for (int i = 0; i < enemiesCount; i++)
			{
				enemiesSpawnData[i] = EnemyTypesList[Random.Range(0, EnemyTypesList.Length)];
				enemiesSpawnDelays[i] = SpawnDelayRange.GetValue();
			}
			return new LevelData
			{
				UnitsSpawnData = enemiesSpawnData,
				SpawnDelays = enemiesSpawnDelays
			};
		}
	}

	public class LevelDataFactory : IFactory<LevelData>
	{
		private readonly LevelSettings _levelSettings;

		public LevelDataFactory(LevelSettings levelSettings)
		{
			_levelSettings = levelSettings;
		}

		public LevelData Create()
		{
			return _levelSettings.ToLevelData();
		}
	}
}
