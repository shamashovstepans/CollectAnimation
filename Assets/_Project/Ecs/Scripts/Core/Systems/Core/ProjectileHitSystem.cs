using _Project.Ecs.Scripts.Core.Components;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class ProjectileHitSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _projectileHitFilter;
        private EcsPool<Clear> _clearPool;
        private EcsPool<ProjectileHit> _projectileHitPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _projectileHitFilter = _world.Filter<ProjectileHit>().End();
            _clearPool = _world.GetPool<Clear>();
            _projectileHitPool = _world.GetPool<ProjectileHit>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var hit in _projectileHitFilter)
            {
                var projectileHit = _projectileHitPool.Get(hit);
                var projectileEntity = projectileHit.ProjectileEntity;
                _clearPool.Add(projectileEntity);
                _clearPool.Add(hit);
            }
        }
    }
}