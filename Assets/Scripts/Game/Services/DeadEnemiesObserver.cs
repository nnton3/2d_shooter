using Assets.Scripts.Game.Level;
using Assets.Scripts.Game.Units.AI;
using System;
using Zenject;

namespace Assets.Scripts.Game.Services
{
	public class DeadEnemiesObserver : IDisposable
	{
		public event Action OnAllEnemiesKilled;
		private EnemySpawner _enemySpawner;
		private int _targetKills;
		private int _killCounter;

		[Inject]
		public void Construct(EnemySpawner enemySpawner, LevelData levelData)
		{
			_enemySpawner = enemySpawner;
			_enemySpawner.OnEnemySpawned += SubscribeToEnemyDead;
			_targetKills = levelData.UnitsSpawnData.Length;
		}

		private void SubscribeToEnemyDead(EnemyUnit enemy)
		{
			enemy.OnDead += RegistryKill(enemy);
		}

		private Action RegistryKill(EnemyUnit enemy)
		{
			return () =>
			{
				enemy.OnDead -= RegistryKill(enemy);
				_killCounter++;
				if (_killCounter < _targetKills)
					return;
				OnAllEnemiesKilled?.Invoke();
			};
		}

		public void Dispose()
		{
			if (_enemySpawner == null) 
				return;
			_enemySpawner.OnEnemySpawned -= SubscribeToEnemyDead;
			OnAllEnemiesKilled = null;
		}
	}
}
