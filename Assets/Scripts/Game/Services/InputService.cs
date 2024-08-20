using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Services
{
	public class InputService : MonoBehaviour
	{
		public Action<float>
			OnMoveHorizontal,
			OnMoveVertical;

		private void Update()
		{
			OnMoveHorizontal?.Invoke(Input.GetAxisRaw("Horizontal"));
			OnMoveVertical?.Invoke(Input.GetAxisRaw("Vertical"));
		}
	}
}
