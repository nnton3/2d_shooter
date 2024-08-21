using Assets.Scripts.Game.Services;
using Assets.Scripts.Game.Unit.Components;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Game.Units.Components
{
	public class MoveComponent : IDisposable
	{
		public float Speed => _strategy.Speed;
		protected Rigidbody2D _rb;
		private TickService _tickService;
		private IMoveStrategy _strategy;

		public MoveComponent(Rigidbody2D rb, TickService tickService, IMoveStrategy strategy)
		{
			_rb = rb;
			_tickService = tickService;
			_tickService.OnFixedTick += FixedTick;
			_strategy = strategy;
		}

		private void FixedTick()
		{
			Vector2 newPosition = _strategy.GetPosition();
			_rb.MovePosition(newPosition);
		}

		public void Dispose()
		{
			_tickService.OnFixedTick -= FixedTick;
		}
	}

	public class MoveComponentFactory : IFactory<Rigidbody2D, IMoveStrategy, MoveComponent>
	{
		private TickService _tickService;

		public MoveComponentFactory(TickService tickService)
		{
			_tickService = tickService;
		}

		public MoveComponent Create(Rigidbody2D rb, IMoveStrategy strategy)
		{
			return new MoveComponent(rb, _tickService, strategy);
		}
	}
}
