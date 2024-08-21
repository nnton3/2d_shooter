using Assets.Scripts.Game.Unit.Components;
using UnityEngine;

namespace Assets.Scripts.Game.Units.AI
{
	public class EnemyMoveStrategy : IMoveStrategy
	{
		public float Speed => _speed;
		private float _speed;
		private Transform _transform;
		private readonly Vector2 _movement;

		public EnemyMoveStrategy(Transform transform, float speed)
		{
			_transform = transform;
			_movement = new Vector2(0f, -speed);
			_speed = speed;
		}

		public Vector2 GetPosition()
		{
			return (Vector2)_transform.position + _movement * Time.fixedDeltaTime;
		}
	}
}
