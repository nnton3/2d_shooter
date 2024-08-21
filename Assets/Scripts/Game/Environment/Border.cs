using Assets.Scripts.Game.Units.AI;
using System;
using UnityEngine;

namespace Assets.Scripts.Game.Environment
{
	public class Border : MonoBehaviour 
	{
		public event Action<EnemyUnit> OnDamaged;

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (!collision.gameObject.TryGetComponent<EnemyUnit>(out var enemy)) 
				return;
			OnDamaged?.Invoke(enemy);
		}

		private void OnDestroy()
		{
			OnDamaged = null;
		}
	}
}
