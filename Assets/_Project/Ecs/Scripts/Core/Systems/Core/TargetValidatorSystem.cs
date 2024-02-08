using _Project.Ecs.Scripts.Core.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class TargetValidatorSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<Target> _targetPool;
        private EcsPool<FindTarget> _findTargetPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<Target>().End();
            _targetPool = _world.GetPool<Target>();
            _findTargetPool = _world.GetPool<FindTarget>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var target = _targetPool.Get(entity);
                var targetEntityPacked = target.TargetEntityPacked;

                if (!targetEntityPacked.Unpack(_world, out _))
                {
                    _targetPool.Del(entity);
                    _findTargetPool.Add(entity);
                }
            }
        }
    }
}