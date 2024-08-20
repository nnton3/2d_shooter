using Assets.Scripts.Game.Units.AI;
using System;
using UnityEngine;

namespace Assets.Scripts.Game.Unit.Player
{
	public class EnemyDetector : MonoBehaviour
	{
		public Action<EnemyUnit> OnTargetDetected;
		[SerializeField] private CircleCollider2D _trigger;

		public void Init(float radius)
		{
			_trigger.radius = radius;
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			var enemy = collider.gameObject.GetComponent<EnemyUnit>();
			if (enemy == null) return;
			OnTargetDetected?.Invoke(enemy);
		}
	}
}
