using Assets.Scripts.Game.Environment;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Units.AI
{
	public class EnemyUnit : Unit
	{
		private const int DEFAULT_DAMAGE = 1;
		public int DefaultDamage => DEFAULT_DAMAGE;
		public float Speed => _moveComponent.Speed;
		public Rigidbody2D Rigidbody => _rb;

		[SerializeField] private Rigidbody2D _rb;
		private Border _border;

		[Inject]
		public void Construct(Border border)
		{
			_border = border;
			_border.OnDamaged += CollisionWithBorder;
		}

		public void ApplyDamage(int value)
		{
			_health.Reduce(value);
		}

		private void CollisionWithBorder(EnemyUnit unit)
		{
			if (unit != this) 
				return;
			RemoveFromBattlefield();
		}

		protected override void Dispose()
		{
			base.Dispose();
			_border.OnDamaged -= CollisionWithBorder;
		}
	}
}
