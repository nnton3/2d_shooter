using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Helpers
{
	[Serializable]
	public class FloatRangeValue
	{
		[SerializeField] private float _min;
		[SerializeField] private float _max;

		public float GetValue()
		{
			return Random.Range(_min, _max);
		}
	}

	[Serializable]
	public class IntRangeValue
	{
		[SerializeField] private int _min;
		[SerializeField] private int _max;

		public int GetValue()
		{
			return Random.Range(_min, _max + 1);
		}
	}
}
