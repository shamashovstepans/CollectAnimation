using _Project.Ecs.Scripts.Core.Common.View;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Configs;
using _Project.Scripts.Ecs.Core.Common;
using _Project.Scripts.Ecs.Dependencies;
using _Project.Scripts.Ecs.View;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Scripts.Ecs.Systems
{
    internal class EnemySpawnerSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private readonly EnemySpawnerConfig _config;
        private readonly IEcsWorldView _worldView;
        private readonly IViewFactory _viewFactory;

        private EcsWorld _world;
        private EcsPool<EnemyTag> _enemyTagPool;
        private EcsPool<FollowTarget> _followPool;
        private EcsPool<ObjectRigidbody> _objectRigidbodyPool;
        private EcsPool<PhysicalBody> _physicalBodyPool;
        private EcsPool<ObjectTransform> _objectTransformPool;
        private EcsPool<Health> _healthPool;

        private float _timeToSpawn;

        public EnemySpawnerSystem(EnemySpawnerConfig config, IEcsWorldView worldView, IViewFactory viewFactory)
        {
            _config = config;
            _worldView = worldView;
            _viewFactory = viewFactory;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _enemyTagPool = _world.GetPool<EnemyTag>();
            _followPool = _world.GetPool<FollowTarget>();
            _objectRigidbodyPool = _world.GetPool<ObjectRigidbody>();
            _physicalBodyPool = _world.GetPool<PhysicalBody>();
            _objectTransformPool = _world.GetPool<ObjectTransform>();
            _healthPool = _world.GetPool<Health>();

            _timeToSpawn = _config.SpawnInterval;
        }

        public void Run(IEcsSystems systems)
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

            _objectTransformPool.Add(enemy);
            _enemyTagPool.Add(enemy);
            _followPool.Add(enemy);
            _objectRigidbodyPool.Add(enemy);
            ref var physicalBody = ref _physicalBodyPool.Add(enemy);

            var spawnPosition = _worldView.GetRandomBorderPoint();
            var view = _viewFactory.Create<IEcsPhysicalBodyView>(enemy, ViewConst.Enemy, spawnPosition, Quaternion.identity, _worldView.EnemiesParent);

            physicalBody.View = view;
            
            ref var health = ref _healthPool.Add(enemy);
            health.Value = _config.Health;
        }
    }
}