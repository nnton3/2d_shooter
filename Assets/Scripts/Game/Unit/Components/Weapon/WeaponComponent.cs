using Assets.Scripts.Game.Services;
using Assets.Scripts.Game.Unit.Player;
using Assets.Scripts.Game.Units.AI;
using Assets.Scripts.ScriptableObjects.Player;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using System;
using Assets.Scripts.Game.Environment;
using Assets.Scripts.Game.Units.Player;
using Assets.Scripts.Game.Units.Components.Weapon;

namespace Assets.Scripts.Game.Units.Components
{
	public class WeaponComponent
	{
		private EnemyDetector _enemyDetector;
		private CoroutineService _coroutineService;
		private Border _border;
		private PlayerUnit _player;
		private Bullet.Factory _bulletFactory;
		private Queue<EnemyUnit> _targets;
		private EnemyUnit _currentTarget;
		private float _fireDelay;
		private float _bulletSpeed;
		private int _damage;
		private Coroutine _fireRoutine;

		[Inject]
		public void Construct(WeaponSettings weaponSettings, EnemyDetector enemyDetector, CoroutineService coroutineService, Border border, PlayerUnit player, Bullet.Factory bulletFactory)
		{
			_coroutineService = coroutineService;
			_border = border;
			_enemyDetector = enemyDetector;
			_player = player;
			_bulletFactory = bulletFactory;
			_enemyDetector.OnTargetDetected += TargetDetectedHandler;

			_fireDelay = 1f / weaponSettings.Speed;
			_bulletSpeed = weaponSettings.BulletSpeed;
			_damage = weaponSettings.Damage;

			_targets = new();
		}

		private void TargetDetectedHandler(EnemyUnit enemy)
		{
			enemy.OnDead += TargetDeadHander;
			_targets.Enqueue(enemy);
			TryStartFire();
		}

		private void TargetDeadHander()
		{
			
		}

		private void TryStartFire()
		{
			if (_fireRoutine != null) return;
			if (_targets.Count == 0) return;
			if (!TrySelectTarget()) return;
			_fireRoutine = _coroutineService.StartCoroutine(FireRoutine());
		}

		private bool TrySelectTarget()
		{
			while (_targets.Count > 0)
			{
				var tmpTarget = _targets.Dequeue();
				if (!TargetIsValid(tmpTarget)) continue;

				_currentTarget = tmpTarget;
				break;
			}

			return _currentTarget != null;
		}

		private bool TargetIsValid(EnemyUnit target)
		{
			var targetPos = target.transform.position;
			var bulletPos = _player.transform.position;
			var borderPos = _border.transform.position;

			var bulletDirection = targetPos - bulletPos;
			var normalizedDirection = bulletDirection.normalized;

			var bulletVelocity = (Vector2)normalizedDirection * _bulletSpeed;
			var targetVelocity = target.Speed * Vector2.down;
			var relativeSpeed = (bulletVelocity - targetVelocity).magnitude;

			var bulletToEnemyDistance = Vector3.Distance(targetPos, bulletPos);
			var timeToCollision = bulletToEnemyDistance / relativeSpeed;

			var enemyToBorderDistance = Mathf.Abs(targetPos.y - borderPos.y);
			var enemyMoveTime = enemyToBorderDistance / target.Speed;

			return enemyMoveTime > timeToCollision;
		}

		private IEnumerator FireRoutine()
		{
			while (TargetIsValid(_currentTarget))
			{
				if (TryToKill()) break;
				yield return new WaitForSeconds(_fireDelay);
			}
			_fireRoutine = null;
			TryStartFire();
		}

		private bool TryToKill()
		{
			var bullet = _bulletFactory.Create();
			bullet.transform.position = _player.transform.position;
			bullet.Init(_damage, _bulletSpeed, _currentTarget.transform);
			return _currentTarget.CurrentHealth <= _damage;
		}
	}
}
