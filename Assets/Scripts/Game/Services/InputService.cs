using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Services
{
	public class InputService : IDisposable
	{
		public event Action<float> OnMoveHorizontal;
		public event Action<float> OnMoveVertical;
		private TickService _tickService;

		[Inject]
		public void Construct(TickService tickService)
		{
			_tickService = tickService;
			_tickService.OnTick += Tick;
		}

		public void Tick()
		{
			OnMoveHorizontal?.Invoke(Input.GetAxisRaw("Horizontal"));
			OnMoveVertical?.Invoke(Input.GetAxisRaw("Vertical"));
		}

		public void Dispose()
		{
			_tickService.OnTick -= Tick;
			OnMoveHorizontal = null;
			OnMoveVertical = null;
		}
	}
}
