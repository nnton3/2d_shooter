using UnityEngine;

namespace Assets.Scripts.Game.Unit.Components
{
	public interface IMoveStrategy
	{
		float Speed { get; }
		Vector2 GetPosition();
	}
}
