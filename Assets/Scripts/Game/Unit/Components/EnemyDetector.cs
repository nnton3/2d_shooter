using Assets.Scripts.Game.Units.AI;
using Assets.Scripts.Game.Units.Player;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.ScriptableObjects.Player;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Unit.Player
{
	public class EnemyDetector : MonoBehaviour
	{
		public event Action<EnemyUnit> OnTargetDetected;
		[SerializeField] private CircleCollider2D _trigger;

		[Inject]
		public void Construct(WeaponSettings weaponSettings)
		{
			_trigger.radius = weaponSettings.Range;
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (!collider.gameObject.TryGetComponent<EnemyUnit>(out var enemy)) 
				return;
			OnTargetDetected?.Invoke(enemy);
		}

		private void OnDestroy()
		{
			OnTargetDetected = null;
		}
	}

	public class EnemyDetectorFactory : IFactory<EnemyDetector>
	{
		private IInstantiator _instantiator;
		private PlayerUnit _player;
		private EnemyDetector _prefab;

		public EnemyDetectorFactory(IInstantiator instantiator, PlayerUnit player, GamePrefabsRepository prefabsRepository)
		{
			_instantiator = instantiator;
			_player = player;
			_prefab = prefabsRepository.EnemyDetector;
		}

		public EnemyDetector Create()
		{
			var instance = _instantiator.InstantiatePrefabForComponent<EnemyDetector>(_prefab, _player.transform);
			return instance;
		}
	}
}
