using UnityEngine;

namespace Assets.Scripts.Game.Services
{
	public class HierarchyService : MonoBehaviour
	{
		public Transform PlayerSpawnPosition => _playerSpawnPoint;
		[SerializeField] private Transform _playerSpawnPoint;
		public Transform[] EnemySpawnPoints => _enemySpawnPoints;
		[SerializeField] private Transform[] _enemySpawnPoints;
	}
}
