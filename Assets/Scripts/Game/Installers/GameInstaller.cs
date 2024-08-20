using Assets.Scripts.Game.Environment;
using Assets.Scripts.Game.Units.Player;
using Assets.Scripts.Game.Units;
using UnityEngine;
using Zenject;
using Assets.Scripts.Game.Services;
using Assets.Scripts.ScriptableObjects.Units;
using Assets.Scripts.ScriptableObjects.Player;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Game.Units.AI;
using Assets.Scripts.Game.UI;
using System;
using Assets.Scripts.Game.Level;
using Assets.Scripts.Game.Units.Components.Weapon;
using Assets.Scripts.Game.Unit.Player;
using Assets.Scripts.Game.Units.Components;

namespace Assets.Scripts.Game.Installers
{
	public class GameInstaller : MonoInstaller
	{
		[Header("Settings")]
		[SerializeField] private PlayerSpawnData _playerSpawnData;
		[SerializeField] private WeaponSettings _weapon;
		[SerializeField] private LevelSettings _level;
		[SerializeField] private EnemyDetector _enemyDetectorPref;

		[Header("Services")]
		[SerializeField] private HierarchyService _hierarchyService;
		[SerializeField] private InputService _inputService;
		[SerializeField] private CoroutineService _coroutineService;

		[Header("Environment")]
		[SerializeField] private Border _border;

		[Header("UI")]
		[SerializeField] private GamePanel _gamePanel;
		[SerializeField] private RestartPanel _restartPanel;

		public override void InstallBindings()
		{
			RegistryEnvironment();
			RegistrySettings();
			RegistryServices();
			RegistryPlayer();
			RegistryUI();
			RegistryLevelData();
			ResitryGameServices();
		}

		private void ResitryGameServices()
		{
			Container.Bind<EnemySpawner>().AsSingle().NonLazy();
			Container.Bind<DeadEnemiesObserver>().AsSingle().NonLazy();
			Container.Bind<GameController>().AsSingle().NonLazy();
		}

		private void RegistryLevelData()
		{
			var levelData = _level.ToLevelData();
			Container.Bind<LevelData>().FromInstance(levelData).AsSingle().NonLazy();
		}

		private void RegistrySettings()
		{
			Container.Bind<PlayerSpawnData>().FromScriptableObject(_playerSpawnData).AsSingle();
			Container.Bind<WeaponSettings>().FromScriptableObject(_weapon).AsSingle();
			Container.BindFactory<Bullet, Bullet.Factory>().FromComponentInNewPrefab(_weapon.BulletPref);
		}

		private void RegistryServices()
		{
			Container.Bind<EnemyUnit>().AsTransient();
			Container.Bind<IUnitFactory>().To<UnitsFactory>().AsSingle();
			Container.Bind<InputService>().FromInstance(_inputService).AsSingle().NonLazy();
			Container.Bind<CoroutineService>().FromInstance(_coroutineService).AsSingle().NonLazy();
			Container.Bind<HierarchyService>().FromInstance(_hierarchyService).AsSingle().NonLazy();
		}

		private void RegistryPlayer()
		{
			var unitFactory = Container.Resolve<IUnitFactory>();
			var player = unitFactory.CreateUnit<PlayerUnit>(_playerSpawnData, _hierarchyService.PlayerSpawnPosition);
			Container.Bind<PlayerUnit>().FromInstance(player).AsSingle();
			var enemyDetector = Container.InstantiatePrefabForComponent<EnemyDetector>(_enemyDetectorPref, player.transform);
			enemyDetector.Init(_weapon.Range);
			Container.Bind<EnemyDetector>().FromInstance(enemyDetector).AsSingle().NonLazy();
			Container.Bind<WeaponComponent>().AsSingle().NonLazy();
		}

		private void RegistryEnvironment()
		{
			Container.Bind<Border>().FromInstance(_border).AsSingle().NonLazy();
		}

		private void RegistryUI()
		{
			Container.Bind<GamePanel>().FromInstance(_gamePanel).AsSingle();
			Container.Bind<RestartPanel>().FromInstance(_restartPanel).AsSingle();
		}
	}
}
