using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Services
{
	public class HierarchyService : MonoBehaviour
	{
		public Transform PlayerSpawnPosition => _playerSpawnPoint;
		[SerializeField] private Transform _playerSpawnPoint;
		public List<Transform> EnemySpawnPoints => _enemySpawnPoints;
		[SerializeField] private List<Transform> _enemySpawnPoints;
	}
}
