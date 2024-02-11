using _Project.Ecs.Scripts.Core.Components;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class CleanDetectionSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _healthFilter;
        private EcsFilter _delayFilter;
        private EcsPool<Clear> _clearPool;
        private EcsPool<Health> _healthPool;
        private EcsPool<DestroyAfterDelay> _delayPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _healthFilter = _world.Filter<Health>().End();
            _delayFilter = _world.Filter<DestroyAfterDelay>().End();
            _clearPool = _world.GetPool<Clear>();
            _healthPool = _world.GetPool<Health>();
            _delayPool = _world.GetPool<DestroyAfterDelay>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _healthFilter)
            {
                var health = _healthPool.Get(entity);
                if (health.Value <= 0)
                {
                    _clearPool.Add(entity);
                }
            }
            
            foreach (var entity in _delayFilter)
            {
                var delay = _delayPool.Get(entity);
                delay.DelayLeft -= Time.deltaTime;
                if (delay.DelayLeft <= 0)
                {
                    _world.DelEntity(entity);
                }
            }
        }
    }
}