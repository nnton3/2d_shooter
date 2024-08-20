using Assets.Scripts.Game.Units;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Units
{
    [CreateAssetMenu(fileName = "SpawnData", menuName = "Game/Units/SpawnData")]
    public class UnitSpawnData : ScriptableObject
    {
        public Unit Prefab;
        [Min(1)] public int Health = 10;
        public float Speed = 1f;
    }
}
