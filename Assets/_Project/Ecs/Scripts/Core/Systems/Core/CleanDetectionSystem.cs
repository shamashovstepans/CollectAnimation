using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class CleanDetectionSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<Clear> _clearView;
        private EcsPool<Health> _healthPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<Health>().End();
            _clearView = _world.GetPool<Clear>();
            _healthPool = _world.GetPool<Health>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var health = _healthPool.Get(entity);
                if (health.Value <= 0)
                {
                    _clearView.Add(entity);
                }
            }
        }
    }
}