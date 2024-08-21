using Assets.Scripts.Game.Units.Components;
using Assets.Scripts.Game.Units.Player;
using Assets.Scripts.ScriptableObjects.Units;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.UI
{
	public class HelathView : MonoBehaviour
	{
		[SerializeField] private TMP_Text _healthField;
		private PlayerUnit _player;

		[Inject]
		public void Construct(PlayerSpawnData playerData, PlayerUnit player)
		{
			_player = player;
			_player.OnHealthChanged += UpdateHealthText;
			UpdateHealthText(playerData.Health, playerData.Health);
		}

		private void UpdateHealthText(HealthData info)
		{
			_healthField.text = $"Health {info.Current}/{info.Max}";			
		}

		private void UpdateHealthText(int max, int current)
		{
			_healthField.text = $"Health {current}/{max}";
		}

		private void OnDestroy()
		{
			_player.OnHealthChanged -= UpdateHealthText;
		}
	}
}
