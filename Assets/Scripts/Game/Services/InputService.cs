using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Services
{
	public class InputService : IDisposable, ITickable
	{
		public event Action<float> OnMoveHorizontal;
		public event Action<float> OnMoveVertical;

		void ITickable.Tick()
		{
			OnMoveHorizontal?.Invoke(Input.GetAxisRaw("Horizontal"));
			OnMoveVertical?.Invoke(Input.GetAxisRaw("Vertical"));
		}

		public void Dispose()
		{
			OnMoveHorizontal = null;
			OnMoveVertical = null;
		}
	}
}
