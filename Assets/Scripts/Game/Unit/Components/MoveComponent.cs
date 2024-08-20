using System;
using UnityEngine;

namespace Assets.Scripts.Game.Units.Components
{
	public abstract class MoveComponent : IDisposable
	{
		protected Rigidbody2D _rb;
		protected readonly float _speed;
		protected Action _onFixedUpdate;

		public MoveComponent(Rigidbody2D rb, float speed, ref Action OnFixedUpdate)
		{
			_rb = rb;
			_speed = speed;
			OnFixedUpdate += FixedUpdate;
			_onFixedUpdate = OnFixedUpdate;
		}

		private void FixedUpdate()
		{
			Vector2 newPosition = GetNewPosition();
			_rb.MovePosition(newPosition);
		}

		protected abstract Vector2 GetNewPosition();

		public virtual void Dispose()
		{
			_onFixedUpdate -= FixedUpdate;
		}
	}
}
