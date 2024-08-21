using Assets.Scripts.Game.Units.Components;
using System;
using UnityEngine;

namespace Assets.Scripts.Game.Units
{
	public class Unit : MonoBehaviour
	{
		public event Action OnDead;
		public int CurrentHealth => _health.Current;
		protected HealthComponent _health;
		protected MoveComponent _moveComponent;

		public virtual void Init(MoveComponent moveComponent, HealthComponent health)
		{
			_moveComponent = moveComponent;
			_health = health;
			_health.IsDead += RemoveFromBattlefield;
		}

		protected void RemoveFromBattlefield()
		{
			OnDead?.Invoke();
			Destroy(gameObject);
		}

		protected virtual void Dispose()
		{
			_moveComponent.Dispose();
			_health.IsDead -= RemoveFromBattlefield;
			_health.Dispose();
		}

		private void OnDestroy()
		{
			OnDead = null;
			Dispose();
		}
	}
}