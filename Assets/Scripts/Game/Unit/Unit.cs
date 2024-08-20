using Assets.Scripts.Game.Units.Components;
using System;
using UnityEngine;

namespace Assets.Scripts.Game.Units
{
	public class Unit : MonoBehaviour
	{
		public Action OnDead;
		public float Speed => _speed;
		public int CurrentHealth => _health.Current;
		protected HealthComponent _health;
		protected MoveComponent _moveComponent;
		protected float _speed;

		public virtual void Init(int maxHealth, float speed)
		{
			_health = new HealthComponent(maxHealth);
			_health.IsDead += DeadHandler;
			_speed = speed;
		}

		protected void DeadHandler()
		{
			Debug.Log($"Unit is dead");
			OnDead?.Invoke();
			Destroy(gameObject);
		}
	}
}