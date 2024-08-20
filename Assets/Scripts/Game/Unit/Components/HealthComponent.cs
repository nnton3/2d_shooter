using System;
using UnityEngine;

namespace Assets.Scripts.Game.Units.Components
{
	public class HealthComponent
	{
		public event Action IsDead;
		public int Current => _current;

		private int _max;
		private int _current;

		public HealthComponent(int max)
		{
			_max = max;
			_current = max;
		}

		public void Reduce(int value)
		{
			_current = Mathf.Max(0, _current - value);
			if (_current == 0)
				IsDead?.Invoke();
		}

		public (int, int) GetInfo() => (_max, _current);
	}
}
