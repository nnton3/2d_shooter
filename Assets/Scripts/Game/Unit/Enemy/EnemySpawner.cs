using Assets.Scripts.Game.Level;
using Assets.Scripts.Game.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Game.Units.AI
{
	public class EnemySpawner
	{
		public Action<EnemyUnit> OnEnemySpawned;

		private LevelData _levelData;
		private CoroutineService _coroutineService;
		private Coroutine _spawnCycleRoutine;
		private IUnitFactory _unitFactory;
		private List<Transform> _spawnPoints;
		private List<EnemyUnit> _enemyInstances = new();

		[Inject]
		public void Construct(CoroutineService coroutineService, IUnitFactory unitFactory, HierarchyService hierarchyService, LevelData levelData)
		{
			_coroutineService = coroutineService;
			_unitFactory = unitFactory;
			_spawnPoints = hierarchyService.EnemySpawnPoints;
			_levelData = levelData;
		}

		public void Start()
		{
			_spawnCycleRoutine = _coroutineService.StartCoroutine(SpawnCycleRoutine());
		}

		private IEnumerator SpawnCycleRoutine()
		{
			for (int i = 0; i < _levelData.UnitsSpawnData.Count; i++)
			{
				var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
				var unitInstance = _unitFactory.CreateUnit<EnemyUnit>(_levelData.UnitsSpawnData[i], spawnPoint);
				_enemyInstances.Add(unitInstance);
				unitInstance.OnDead += () => OnEnemyDeadHandler(unitInstance);
				OnEnemySpawned?.Invoke(unitInstance);
				yield return new WaitForSeconds(_levelData.SpawnDelays[i]);
			}
		}

		private void OnEnemyDeadHandler(EnemyUnit unitInstance)
		{
			_enemyInstances.Remove(unitInstance);
		}

		public void Stop()
		{
			_coroutineService.StopCoroutine(_spawnCycleRoutine);
			foreach (var enemy in _enemyInstances)
			{
				if (enemy == null) continue;
				UnityEngine.Object.Destroy(enemy.gameObject);
			}
		}
	}
}
