using Assets.Scripts.Game.Level;
using Assets.Scripts.Game.Units;
using Assets.Scripts.Game.Units.AI;
using System;
using Zenject;

namespace Assets.Scripts.Game.Services
{
	public class DeadEnemiesObserver : IDisposable
	{
		public Action OnAllEnemiesKilled;
		private EnemySpawner _enemySpawner;
		private int _targetKills;
		private int _killCounter;

		[Inject]
		public void Construct(EnemySpawner enemySpawner, LevelData levelData)
		{
			_enemySpawner = enemySpawner;
			_enemySpawner.OnEnemySpawned += EnemySpawnedHandler;
			_targetKills = levelData.UnitsSpawnData.Count;
		}

		private void EnemySpawnedHandler(EnemyUnit enemy)
		{
			enemy.OnDead += EnemyDeadHandler;
		}

		private void EnemyDeadHandler()
		{
			_killCounter++;
			if (_killCounter < _targetKills) return;
			OnAllEnemiesKilled?.Invoke();
		}

		public void Dispose()
		{
			if (_enemySpawner == null) return;
			_enemySpawner.OnEnemySpawned -= EnemySpawnedHandler;
		}
	}
}
