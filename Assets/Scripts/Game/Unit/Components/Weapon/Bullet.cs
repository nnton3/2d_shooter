﻿using Assets.Scripts.Game.Units.AI;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Units.Components.Weapon
{
	public class Bullet : MonoBehaviour
	{
		[SerializeField] private Rigidbody2D _rb;
		private int _damage;
		private float _speed;
		private Transform _target;

		private void FixedUpdate()
		{
			if (_target == null)
			{
				Destroy(gameObject);
				return;
			}
			var direction = (_target.position - transform.position).normalized;
			var newPosition = (Vector2)(transform.position + direction * _speed * Time.fixedDeltaTime);
			var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

			_rb.rotation = angle;
			_rb.MovePosition(newPosition);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (!collision.gameObject.TryGetComponent<EnemyUnit>(out var enemy)) 
				return;
			enemy.ApplyDamage(_damage);
			Destroy(gameObject);
		}

		public void Init(int damage, float speed, Transform target)
		{
			_damage = damage;
			_speed = speed;
			_target = target;
		}

	}
	
	public class BulletFactory : PlaceholderFactory<Bullet> { }
}
