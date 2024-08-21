using Assets.Scripts.Game.Unit.Player;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
	[CreateAssetMenu(fileName = "GamePrefabsRepository", menuName = "Game/GamePrefabsRepository")]
	public class GamePrefabsRepository : ScriptableObject
	{
		public EnemyDetector EnemyDetector => _enemyDetectorPrefab;
		[SerializeField] private EnemyDetector _enemyDetectorPrefab;
	}
}
