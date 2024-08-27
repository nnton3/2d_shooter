using Assets.Scripts.Game.Unit.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Units.Components
{
	public class MoveComponent : IFixedTickable, IDisposable
	{
		public float Speed => _strategy.Speed;
		protected Rigidbody2D _rb;
		private IMoveStrategy _strategy;
		private TickableManager _tickableManager;

		public MoveComponent(Rigidbody2D rb, IMoveStrategy strategy, TickableManager tickableManager)
		{
			_rb = rb;
			_strategy = strategy;
			_tickableManager = tickableManager;
			_tickableManager.AddFixed(this);
		}

		void IFixedTickable.FixedTick()
		{
			Vector2 newPosition = _strategy.GetPosition();
			_rb.MovePosition(newPosition);
		}

		public void Dispose()
		{
			_tickableManager.RemoveFixed(this);
		}
	}

	public class MoveComponentFactory : IFactory<Rigidbody2D, IMoveStrategy, MoveComponent>
	{
		private readonly IInstantiator _instantiator;

		public MoveComponentFactory(IInstantiator instantiator)
		{
			_instantiator = instantiator;
		}

		public MoveComponent Create(Rigidbody2D rb, IMoveStrategy strategy)
		{
			var instance = _instantiator.Instantiate<MoveComponent>(new List<object>() { rb, strategy });
			return instance;
		}
	}
}
