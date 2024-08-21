using Assets.Scripts.Game.Level;
using Assets.Scripts.Game.Services;
using Assets.Scripts.Game.Unit.Components;
using Assets.Scripts.Game.Units.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Game.Units.AI
{
	public class EnemySpawner : IDisposable
	{
		public event Action<EnemyUnit> OnEnemySpawned;

		private LevelData _levelData;
		private CoroutineService _coroutineService;
		private Coroutine _spawnCycleRoutine;
		private IUnitFactory _unitFactory;
		private Transform[] _spawnPoints;
		private List<EnemyUnit> _enemyInstances = new();
		private IFactory<Rigidbody2D, IMoveStrategy, MoveComponent> _moveComponentFactory;

		[Inject]
		public void Construct(CoroutineService coroutineService, IUnitFactory unitFactory, HierarchyService hierarchyService, LevelData levelData, IFactory<Rigidbody2D, IMoveStrategy, MoveComponent> moveComponentFactory)
		{
			_moveComponentFactory = moveComponentFactory;
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
			for (int i = 0; i < _levelData.UnitsSpawnData.Length; i++)
			{
				var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
				var unitInstance = _unitFactory.CreateUnit<EnemyUnit>(_levelData.UnitsSpawnData[i], spawnPoint);
				_enemyInstances.Add(unitInstance);
				var moveStrategy = new EnemyMoveStrategy(unitInstance.transform, _levelData.UnitsSpawnData[i].Speed);
				var moveComponent = _moveComponentFactory.Create(unitInstance.Rigidbody, moveStrategy);
				var healthComponent = new HealthComponent(_levelData.UnitsSpawnData[i].Health);
				unitInstance.Init(moveComponent, healthComponent);
				unitInstance.OnDead += StopListen(unitInstance);
				OnEnemySpawned?.Invoke(unitInstance);
				yield return new WaitForSeconds(_levelData.SpawnDelays[i]);
			}
		}

		private Action StopListen(EnemyUnit unitInstance)
		{
			return () =>
			{
				unitInstance.OnDead -= StopListen(unitInstance);
				_enemyInstances.Remove(unitInstance);
			};
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

		public void Dispose()
		{
			OnEnemySpawned = null;
		}
	}
}
