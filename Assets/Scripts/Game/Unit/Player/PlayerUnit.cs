using Assets.Scripts.Game.Environment;
using Assets.Scripts.Game.Units.AI;
using Assets.Scripts.Game.Units.Components;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Units.Player
{
	public class PlayerUnit : Unit
	{
		public const float SIZE = 1f;
		public Rigidbody2D Rigidbody => _rb;
		public event Action<HealthData> OnHealthChanged;
		[SerializeField] private Rigidbody2D _rb;
		private Border _border;

		[Inject]
		public void Construct(Border border) 
		{
			_border = border;
			_border.OnDamaged += ApplyDamage;
		}

		private void ApplyDamage(EnemyUnit enemy)
		{
			_health.Reduce(enemy.DefaultDamage);
			OnHealthChanged?.Invoke(_health.GetInfo());
		}

		protected override void RemoveFromBattlefield()
		{
			_border.OnDamaged -= ApplyDamage;
			OnHealthChanged = null;
			base.RemoveFromBattlefield();
		}
	}
}
