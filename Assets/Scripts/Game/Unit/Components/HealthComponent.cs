﻿using System;
using UnityEngine;

namespace Assets.Scripts.Game.Units.Components
{
	public class HealthComponent : IDisposable
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

		public HealthData GetInfo()
		{
			return new HealthData
			{
				Max = _max,
				Current = _current
			};
		}

		public void Dispose()
		{
			IsDead = null;
		}
	}

	public class HealthData
	{
		public int Current;
		public int Max;
	}
}
