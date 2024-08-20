using Assets.Scripts.Game.Units.AI;
using System;
using UnityEngine;

namespace Assets.Scripts.Game.Environment
{
	public class Border : MonoBehaviour 
	{
		public Action<EnemyUnit> OnDamaged;

		private void OnCollisionEnter2D(Collision2D collision)
		{
			var enemy = collision.gameObject.GetComponent<EnemyUnit>();
			if (enemy == null) return;
			OnDamaged?.Invoke(enemy);
		}
	}
}
