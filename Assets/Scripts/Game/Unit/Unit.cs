using Assets.Scripts.Game.Units.Components;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Units
{
	public class Unit : MonoBehaviour
	{
		public event Action OnDead;
		public int CurrentHealth => _health.Current;
		protected HealthComponent _health;
		protected MoveComponent _moveComponent;
		private TickableManager _tickableManager;

		[Inject]
		public void Construct(TickableManager tickableManager)
		{
			_tickableManager = tickableManager;
		}

		public virtual void Init(MoveComponent moveComponent, HealthComponent health)
		{
			_moveComponent = moveComponent;
			_health = health;
			_health.IsDead += RemoveFromBattlefield;
		}

		protected virtual void RemoveFromBattlefield()
		{
			OnDead?.Invoke();
			OnDead = null;
			_health.IsDead -= RemoveFromBattlefield;
			_health.Dispose();
			_moveComponent.Dispose();
			Destroy(gameObject);
		}
	}
}