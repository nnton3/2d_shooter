using Assets.Scripts.Game.Environment;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Units.AI
{
	public class EnemyUnit : Unit
	{
		public int DefaultDamage => DEFAULT_DAMAGE;
		private const int DEFAULT_DAMAGE = 1;
		[SerializeField] private Rigidbody2D _rb;
		private Action _onFixedUpdate;

		public override void Init(int maxHealth, float speed)
		{
			base.Init(maxHealth, speed);
			_moveComponent = new EnemyMoveComponent(_rb, _speed, ref _onFixedUpdate);
		}

		public void ApplyDamage(int value)
		{
			_health.Reduce(value);
		}

		[Inject]
		public void Construct(Border border)
		{
			border.OnDamaged += CollisionWithBorderHandler;
		}

		private void CollisionWithBorderHandler(EnemyUnit unit)
		{
			if (unit != this) return;
			DeadHandler();
		}

		private void FixedUpdate() => _onFixedUpdate?.Invoke();
	}
}
