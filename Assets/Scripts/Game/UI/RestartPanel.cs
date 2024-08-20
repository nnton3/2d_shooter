using Assets.Scripts.Game.Units.Player;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Game.UI
{
	public class RestartPanel : MonoBehaviour
	{
		[SerializeField] private Button _restartBtn;
		[SerializeField] private CanvasGroup _group;
		private Action _onRestart;

		private void Awake()
		{
			_restartBtn.onClick.AddListener(RestartHandler);
		}

		private void RestartHandler()
		{
			_onRestart?.Invoke();
			_onRestart = null;
		}

		public void Show(Action onRestart)
		{
			_onRestart = onRestart;
			_group.interactable = true;
			_group.blocksRaycasts = true;
			_group.alpha = 1f;
		}

		public void Hide()
		{
			_group.interactable = false;
			_group.blocksRaycasts = false;
			_group.alpha = 0f;
		}
	}
}
