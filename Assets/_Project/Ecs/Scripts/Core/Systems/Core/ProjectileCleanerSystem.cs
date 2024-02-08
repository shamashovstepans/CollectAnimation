using _Project.Ecs.Scripts.Core.Components;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class ProjectileCleanerSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<Clear> _clearPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<Projectile>().Inc<TargetNotFound>().Exc<Clear>().End();
            _clearPool = _world.GetPool<Clear>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                _clearPool.Add(entity);
            }
        }
    }
}