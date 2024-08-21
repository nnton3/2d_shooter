using Assets.Scripts.Game.Environment;
using Assets.Scripts.Game.Services;
using Assets.Scripts.Game.Unit.Components;
using System;
using UnityEngine;

namespace Assets.Scripts.Game.Units.Player
{
	public class PlayerMoveStrategy : IMoveStrategy, IDisposable
	{
		public float Speed => _speed;
		private Transform _transform;
		private Transform _border;
		private Vector2 _movement;
		private Vector2 _minBounds, _maxBounds;
		private Camera _camera;
		private InputService _inputService;
		private float _speed;

		public PlayerMoveStrategy(Transform transform, float speed, InputService inputService, Border border)
		{
			_transform = transform;
			_speed = speed;
			_camera = Camera.main;
			_inputService = inputService;
			_border = border.transform;
			UpdateScreenBounds();
			SubscribeToInput();
		}

		public  Vector2 GetPosition()
		{
			Vector2 newPosition = (Vector2)_transform.position + _movement * _speed * Time.fixedDeltaTime;
			newPosition.x = Mathf.Clamp(newPosition.x, _minBounds.x, _maxBounds.x);
			newPosition.y = Mathf.Clamp(newPosition.y, _minBounds.y, _maxBounds.y);
			return newPosition;
		}

		private void UpdateScreenBounds()
		{
			var playerSizeOffcet = new Vector2(PlayerUnit.SIZE / 2f, PlayerUnit.SIZE / 2f);
			Vector3 screenBottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.transform.position.z));
			Vector3 screenTopRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.transform.position.z));

			_minBounds = new Vector2(screenBottomLeft.x, screenBottomLeft.y) + playerSizeOffcet;
			_maxBounds = new Vector2(screenTopRight.x, _border.position.y) - playerSizeOffcet;
		}

		#region Input handlers
		private void SubscribeToInput()
		{
			_inputService.OnMoveHorizontal += MoveHorizontal;
			_inputService.OnMoveVertical += MoveVertical;
		}

		private void UnsubscribeFromInput()
		{
			_inputService.OnMoveHorizontal -= MoveHorizontal;
			_inputService.OnMoveVertical -= MoveVertical;
		}

		private void MoveHorizontal(float value) => _movement.x = value;
		private void MoveVertical(float value) => _movement.y = value;
		#endregion

		public void Dispose()
		{
			UnsubscribeFromInput();
		}
	}
}
