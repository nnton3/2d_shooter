using UnityEngine;

namespace Assets.Scripts.Game.Services
{
	public class CoroutineService : MonoBehaviour 
	{
		private void OnDestroy()
		{
			StopAllCoroutines();
		}
	}
}
