using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class PlayerTargetingSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filterStanding;
        private EcsFilter _filterMoving;
        private EcsPool<LookAtTarget> _lookAtTargetPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filterStanding = _world.Filter<PlayerTag>().Inc<Standing>().End();
            _filterMoving = _world.Filter<PlayerTag>().Inc<Moving>().End();
            _lookAtTargetPool = _world.GetPool<LookAtTarget>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterStanding)
            {
                if (_lookAtTargetPool.Has(entity))
                {
                    continue;
                }

                _lookAtTargetPool.Add(entity);
            }
            
            foreach (var entity in _filterMoving)
            {
                if (!_lookAtTargetPool.Has(entity))
                {
                    continue;
                }

                _lookAtTargetPool.Del(entity);
            }
        }
    }
}