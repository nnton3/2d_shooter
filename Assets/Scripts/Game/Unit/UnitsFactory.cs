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
		private readonly DiContainer _container;

		public UnitsFactory(DiContainer container)
		{
			_container = container;
		}

		public T CreateUnit<T>(UnitSpawnData data, Transform parent) where T : Unit
		{
			var instance = _container.InstantiatePrefabForComponent<T>(data.Prefab, parent.position, Quaternion.identity, parent);
			instance.Init(data.Health, data.Speed);
			return instance;
		}
	}
}
