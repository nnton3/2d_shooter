using Assets.Scripts.Game.Environment;
using Assets.Scripts.Game.Services;
using Assets.Scripts.Game.Units.AI;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Units.Player
{
	public class PlayerUnit : Unit
	{
		public Action<(int, int)> OnHealthChanged;
		public const float SIZE = 1f;
		[SerializeField] private Rigidbody2D _rb;
		private Action _onFixedUpdate;
		private InputService _inputService;
		private Border _border;

		[Inject]
		public void Construct(InputService inputService, Border border) 
		{
			_inputService = inputService;
			_border = border;
			_border.OnDamaged += BorderDamagedHandler;
		}

		private void BorderDamagedHandler(EnemyUnit enemy)
		{
			_health.Reduce(enemy.DefaultDamage);
			OnHealthChanged?.Invoke(_health.GetInfo());
		}

		public override void Init(int maxHealth, float speed)
		{
			base.Init(maxHealth, speed);
			_moveComponent = new PlayerMoveComponent(_rb, _speed, ref _onFixedUpdate, _inputService, _border);
		}

		private void FixedUpdate() => _onFixedUpdate?.Invoke();
	}
}
