using System;
using Zenject;

namespace Assets.Scripts.Game.Services
{
	public class TickService : ITickable, IFixedTickable, IDisposable
	{
		public event Action OnTick;
		public event Action OnFixedTick;

		public void Tick()
		{
			OnTick?.Invoke();
		}

		public void FixedTick()
		{
			OnFixedTick?.Invoke();
		}

		public void Dispose()
		{
			OnTick = null;
			OnFixedTick = null;
		}
	}
}
