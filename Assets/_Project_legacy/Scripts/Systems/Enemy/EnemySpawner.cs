using System;
using System.Collections.Generic;
using Game.Ground;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Game.Systems.Enemy
{
    internal class EnemySpawner : IInitializable, IDisposable, ITickable
    {
        private readonly EnemySpawnerSettings _settings;
        private readonly EnemiesParent _enemiesParent;
        private readonly IGround _ground;
        private readonly IPlayer _player;

        private readonly List<Enemy> _enemies = new();

        private float _timeSinceLastSpawn;

        public EnemySpawner(
            EnemySpawnerSettings settings,
            EnemiesParent enemiesParent,
            IGround ground,
            IPlayer player)
        {
            _settings = settings;
            _enemiesParent = enemiesParent;
            _ground = ground;
            _player = player;
        }

        public void Initialize()
        {
        }

        public void Dispose()
        {
            foreach (var enemy in _enemies)
            {
                if (enemy == null)
                {
                    continue;
                }

                Object.Destroy(enemy.gameObject);
            }

            _enemies.Clear();
        }

        public void Tick()
        {
            _timeSinceLastSpawn -= Time.deltaTime;

            if (_timeSinceLastSpawn > 0)
            {
                return;
            }

            _timeSinceLastSpawn = _settings.SpawnInterval;
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            var enemy = Object.Instantiate(_settings.EnemyPrefab, _ground.GetRandomBorderPoint(), Quaternion.identity, _enemiesParent.transform);
            _enemies.Add(enemy);
            enemy.Init(_player.Transform, _settings);
        }
    }
}