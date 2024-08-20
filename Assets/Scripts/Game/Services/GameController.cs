﻿using Assets.Scripts.Game.UI;
using Assets.Scripts.Game.Units.AI;
using Assets.Scripts.Game.Units.Player;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Scripts.Game.Services
{
	public class GameController : IDisposable
	{
		private EnemySpawner _enemySpawner;
		private DeadEnemiesObserver _deadEnemiesObserver;
		private PlayerUnit _player;
		private RestartPanel _restartPanel;

		[Inject]
		public void Construct(PlayerUnit player, RestartPanel restartPanel, EnemySpawner enemySpawner, DeadEnemiesObserver deadEnemiesObserver)
		{
			_player = player;
			_player.OnDead += PlayerDeadHandler;
			_restartPanel = restartPanel;
			_enemySpawner = enemySpawner;
			_deadEnemiesObserver = deadEnemiesObserver;
			_deadEnemiesObserver.OnAllEnemiesKilled += LevelEndHandler;

			StartAsync();
		}

		public async Task StartAsync()
		{
			await Task.Delay(1000);
			_enemySpawner.Start();
		}
		
		private void PlayerDeadHandler()
		{
			_enemySpawner.Stop();
			_restartPanel.Show(RestartScene);
		}

		private void LevelEndHandler()
		{
			_enemySpawner.Stop();
			_restartPanel.Show(RestartScene);
		}

		private void RestartScene()
		{
			var currentScene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(currentScene.name);
		}

		public void Dispose()
		{
			if (_deadEnemiesObserver != null)
				_deadEnemiesObserver.OnAllEnemiesKilled -= LevelEndHandler;

			if (_player != null) 
				_player.OnDead -= PlayerDeadHandler;
		}
	}
}
