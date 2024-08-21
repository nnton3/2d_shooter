using Assets.Scripts.ScriptableObjects.Units;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Units
{
	public interface IUnitFactory
	{
		T CreateUnit<T>(UnitSpawnData data, Transform parent) where T : Unit;
	}

	public class UnitsFactory : IUnitFactory
	{
		private readonly IInstantiator _instantiator;

		public UnitsFactory(IInstantiator instantiator)
		{
			_instantiator = instantiator;
		}

		public T CreateUnit<T>(UnitSpawnData data, Transform parent) where T : Unit
		{
			var instance = _instantiator.InstantiatePrefabForComponent<T>(data.Prefab, parent.position, Quaternion.identity, parent);
			return instance;
		}
	}
}
