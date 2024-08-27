using Assets.Scripts.Game.Services;
using Assets.Scripts.Game.Unit.Player;
using Assets.Scripts.Game.Units.AI;
using Assets.Scripts.ScriptableObjects.Player;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using Assets.Scripts.Game.Environment;
using Assets.Scripts.Game.Units.Player;
using Assets.Scripts.Game.Units.Components.Weapon;
using System;

namespace Assets.Scripts.Game.Units.Components
{
	public class WeaponComponent : IDisposable
	{
		private EnemyDetector _enemyDetector;
		private CoroutineService _coroutineService;
		private Border _border;
		private PlayerUnit _player;
		private BulletFactory _bulletFactory;
		private Queue<EnemyUnit> _targets;
		private EnemyUnit _currentTarget;
		private float _fireDelay;
		private float _bulletSpeed;
		private int _damage;
		private Coroutine _fireRoutine;

		[Inject]
		public void Construct(WeaponSettings weaponSettings, EnemyDetector enemyDetector, CoroutineService coroutineService, Border border, PlayerUnit player, BulletFactory bulletFactory)
		{
			_coroutineService = coroutineService;
			_border = border;
			_enemyDetector = enemyDetector;
			_player = player;
			_bulletFactory = bulletFactory;
			_enemyDetector.OnTargetDetected += RegistryTarget;

			_fireDelay = 1f / weaponSettings.Speed;
			_bulletSpeed = weaponSettings.BulletSpeed;
			_damage = weaponSettings.Damage;

			_targets = new();
		}

		private void RegistryTarget(EnemyUnit enemy)
		{
			enemy.OnDead += RemoveTarget(enemy);
			_targets.Enqueue(enemy);
			TryStartFire();
		}

		private Action RemoveTarget(EnemyUnit enemy)
		{
			return () =>
			{
				enemy.OnDead -= RemoveTarget(enemy);
				TryStartFire();
			};
		}

		private void TryStartFire()
		{
			if (_fireRoutine != null) 
				return;
			if (_targets.Count == 0) 
				return;
			if (!TrySelectTarget()) 
				return;
			_fireRoutine = _coroutineService.StartCoroutine(FireRoutine());
		}

		private bool TrySelectTarget()
		{
			while (_targets.Count > 0)
			{
				var tmpTarget = _targets.Dequeue();
				if (tmpTarget == null || !TargetIsValid(tmpTarget)) continue;

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

		public void Dispose()
		{
			_coroutineService.StopCoroutine(_fireRoutine);
			_enemyDetector.OnTargetDetected -= RegistryTarget;
		}
	}
}
