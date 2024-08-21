using Assets.Scripts.Game.Units.Components.Weapon;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects.Player
{
	[CreateAssetMenu(fileName = "_weapon", menuName = "Game/WeaponSettings")]
	public class WeaponSettings : ScriptableObject
	{
		public float Range;
		public float Speed;
		public int Damage;
		public float BulletSpeed;
		public Bullet BulletPrefab;
	}
}
