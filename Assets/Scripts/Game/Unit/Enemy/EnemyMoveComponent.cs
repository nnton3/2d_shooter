using Assets.Scripts.Game.Units.Components;
using System;
using UnityEngine;

namespace Assets.Scripts.Game.Units.AI
{
	public class EnemyMoveComponent : MoveComponent
	{
		private readonly Vector2 _movement;

		public EnemyMoveComponent(Rigidbody2D rb, float speed, ref Action OnFixedUpdate) : 
			base(rb, speed, ref OnFixedUpdate)
		{
			_movement = new Vector2(0f, -speed);
		}

		protected override Vector2 GetNewPosition()
		{
			return _rb.position + _movement * Time.fixedDeltaTime;
		}
	}
}
