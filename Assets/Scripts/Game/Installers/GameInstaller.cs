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
using Assets.Scripts.Game.Level;
using Assets.Scripts.Game.Units.Components.Weapon;
using Assets.Scripts.Game.Unit.Player;
using Assets.Scripts.Game.Units.Components;
using Assets.Scripts.Game.Unit.Components;

namespace Assets.Scripts.Game.Installers
{
	public class GameInstaller : MonoInstaller
	{
		[Header("Settings")]
		[SerializeField] private PlayerSpawnData _playerSpawnData;
		[SerializeField] private WeaponSettings _weapon;
		[SerializeField] private LevelSettings _level;
		[SerializeField] private GamePrefabsRepository _prefabsRepository;

		[Header("Services")]
		[SerializeField] private HierarchyService _hierarchyService;
		[SerializeField] private CoroutineService _coroutineService;

		[Header("Environment")]
		[SerializeField] private Border _border;

		[Header("UI")]
		[SerializeField] private HelathView _gamePanel;
		[SerializeField] private RestartPanel _restartPanel;

		public override void InstallBindings()
		{
			RegistryEnvironment();
			RegistrySettings();
			RegistryServices();
			RegistryPlayer();
			RegistryUI();
			RegistryLevelData();
			RegistryGameServices();
		}

		private void RegistrySettings()
		{
			Container.Bind<PlayerSpawnData>().FromScriptableObject(_playerSpawnData).AsSingle();
			Container.Bind<WeaponSettings>().FromScriptableObject(_weapon).AsSingle();
			Container.BindFactory<Bullet, BulletFactory>().FromComponentInNewPrefab(_weapon.BulletPrefab);
			Container.Bind<GamePrefabsRepository>().FromScriptableObject(_prefabsRepository).AsSingle();
		}

		private void RegistryGameServices()
		{
			Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle().NonLazy();
			Container.Bind<DeadEnemiesObserver>().AsSingle();
			Container.Bind<GameController>().AsSingle().NonLazy();
		}

		private void RegistryLevelData()
		{
			Container.Bind<LevelSettings>().FromInstance(_level).AsSingle().NonLazy();
			Container.Bind<IFactory<LevelData>>().To<LevelDataFactory>().AsSingle().NonLazy();
			Container.Bind<LevelData>().FromMethod(context =>
			{
				var factory = context.Container.Resolve<IFactory<LevelData>>();
				return factory.Create();
			}).AsSingle();
		}

		private void RegistryServices()
		{
			Container.BindInterfacesAndSelfTo<TickService>().AsSingle().NonLazy();
			Container.Bind<IFactory<Rigidbody2D, IMoveStrategy, MoveComponent>>()
				.To<MoveComponentFactory>()
				.AsSingle();
			Container.Bind<EnemyUnit>().AsTransient();
			Container.Bind<IUnitFactory>().To<UnitsFactory>().AsSingle();

			Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
			Container.Bind<CoroutineService>().FromInstance(_coroutineService).AsSingle();
			Container.Bind<HierarchyService>().FromInstance(_hierarchyService).AsSingle();
		}

		private void RegistryPlayer()
		{
			Container.Bind<PlayerUnit>().FromFactory<PlayerFactory>().AsSingle().NonLazy();
			Container.Bind<EnemyDetector>().FromFactory<EnemyDetectorFactory>().AsSingle();
			Container.BindInterfacesAndSelfTo<WeaponComponent>().AsSingle().NonLazy();
		}

		private void RegistryEnvironment()
		{
			Container.Bind<Border>().FromInstance(_border).AsSingle();
		}

		private void RegistryUI()
		{
			Container.Bind<HelathView>().FromInstance(_gamePanel).AsSingle();
			Container.Bind<RestartPanel>().FromInstance(_restartPanel).AsSingle();
		}
	}
}
