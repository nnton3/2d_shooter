using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.UI
{
	public class RestartPanel : MonoBehaviour
	{
		[SerializeField] private Button _restartBtn;
		[SerializeField] private CanvasGroup _group;
		private event Action _onRestart;

		private void Awake()
		{
			_restartBtn.onClick.AddListener(InvokeRestart);
		}

		private void InvokeRestart()
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

		private void OnDestroy()
		{
			_onRestart = null;
		}
	}
}
