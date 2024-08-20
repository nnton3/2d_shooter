using Assets.Scripts.Game.Units.Player;
using Assets.Scripts.ScriptableObjects.Units;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.UI
{
	public class GamePanel : MonoBehaviour
	{
		[SerializeField] private TMP_Text _healthField;

		[Inject]
		public void Construct(PlayerSpawnData playerData, PlayerUnit player)
		{
			player.OnHealthChanged += HealthChangedHandler;
			HealthChangedHandler((playerData.Health, playerData.Health));
		}

		private void HealthChangedHandler((int, int) info)
		{
			_healthField.text = $"Health {info.Item2}/{info.Item1}";			
		}
	}
}
