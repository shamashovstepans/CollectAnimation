using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Configs;
using _Project.Scripts.Ecs.Core.Common;
using _Project.Scripts.Ecs.Dependencies;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Scripts.Ecs.Systems
{
    internal class EnemySpawnerSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private readonly EnemySpawnerConfig _config;
        private readonly IEcsWorldView _worldView;

        private EcsWorld _world;
        private EcsPool<EnemyTag> _enemyTagPool;
        private EcsPool<FollowTarget> _followPool;
        private EcsPool<ObjectRigidbody> _objectRigidbodyPool;
        private EcsPool<PhysicalBody> _physicalBodyPool;
        private EcsPool<ObjectTransform> _objectTransformPool;
        
        private float _timeToSpawn;

        public EnemySpawnerSystem(EnemySpawnerConfig config, IEcsWorldView worldView)
        {
            _config = config;
            _worldView = worldView;
        }

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _enemyTagPool = _world.GetPool<EnemyTag>();
            _followPool = _world.GetPool<FollowTarget>();
            _objectRigidbodyPool = _world.GetPool<ObjectRigidbody>();
            _physicalBodyPool = _world.GetPool<PhysicalBody>();
            _objectTransformPool = _world.GetPool<ObjectTransform>();
            
            _timeToSpawn = _config.SpawnInterval;
        }

        public void Run(EcsSystems systems)
        {
            _timeToSpawn -= Time.deltaTime;
            
            if (_timeToSpawn <= 0)
            {
                _timeToSpawn = _config.SpawnInterval;
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            var enemy = _world.NewEntity();

            var spawnPosition = _worldView.GetRandomBorderPoint();
            var prefab = _config.EnemyPrefab;
            var view = Object.Instantiate(prefab, spawnPosition, Quaternion.identity, _worldView.EnemiesParent);
            var physicalBodyView = view.GetComponent<IEcsPhysicalBodyView>();

            _objectTransformPool.Add(enemy);
            _enemyTagPool.Add(enemy);
            _followPool.Add(enemy);
            _objectRigidbodyPool.Add(enemy);
            ref var physicalBody = ref _physicalBodyPool.Add(enemy);
            physicalBody.View = physicalBodyView;
        }
    }
}