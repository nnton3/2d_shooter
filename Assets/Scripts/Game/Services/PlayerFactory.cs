using Assets.Scripts.Game.Environment;
using Assets.Scripts.Game.Unit.Components;
using Assets.Scripts.Game.Unit.Player;
using Assets.Scripts.Game.Units;
using Assets.Scripts.Game.Units.Components;
using Assets.Scripts.Game.Units.Player;
using Assets.Scripts.ScriptableObjects.Units;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Services
{
	public class PlayerFactory : IFactory<PlayerUnit>
	{
		private IUnitFactory _unitFactory;
		private PlayerSpawnData _spawnData;
		private HierarchyService _hierarchyService;
		private IFactory<Rigidbody2D, IMoveStrategy, MoveComponent> _moveComponentFactory;
		private InputService _inputService;
		private Border _border;

		public PlayerFactory(IUnitFactory unitFactory, PlayerSpawnData spawnData, HierarchyService hierarchyService, IFactory<Rigidbody2D, IMoveStrategy, MoveComponent> moveComponentFactory, InputService inputService, Border border)
		{
			_unitFactory = unitFactory;
			_spawnData = spawnData;
			_hierarchyService = hierarchyService;
			_moveComponentFactory = moveComponentFactory;
			_inputService = inputService;
			_border = border;
		}

		public PlayerUnit Create()
		{
			var player = _unitFactory.CreateUnit<PlayerUnit>(_spawnData, _hierarchyService.PlayerSpawnPosition);
			var moveStrategy = new PlayerMoveStrategy(player.transform, _spawnData.Speed, _inputService, _border);
			var moveComponent = _moveComponentFactory.Create(player.Rigidbody, moveStrategy);
			var healthComponent = new HealthComponent(_spawnData.Health);
			player.Init(moveComponent, healthComponent);
			return player;
		}
	}
}
