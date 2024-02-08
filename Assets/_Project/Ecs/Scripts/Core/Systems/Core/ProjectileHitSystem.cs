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
        private EcsPool<Projectile> _projectilePool;
        private EcsPool<Health> _healthPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _projectileHitFilter = _world.Filter<ProjectileHit>().End();
            _clearPool = _world.GetPool<Clear>();
            _projectileHitPool = _world.GetPool<ProjectileHit>();
            _healthPool = _world.GetPool<Health>();
            _projectilePool = _world.GetPool<Projectile>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var hit in _projectileHitFilter)
            {
                var projectileHit = _projectileHitPool.Get(hit);
                var projectilePackedEntity = projectileHit.ProjectileEntity;

                if (!projectilePackedEntity.Unpack(_world, out var projectileEntity))
                {
                    continue;
                }

                var projectile = _projectilePool.Get(projectileEntity);
                var targetPackedEntity = projectileHit.TargetEntity;

                if (!targetPackedEntity.Unpack(_world, out var targetEntity))
                {
                    continue;
                }

                if (!_healthPool.Has(targetEntity))
                {
                    continue;
                }

                ref var health = ref _healthPool.Get(targetEntity);
                health.Value -= projectile.Damage;

                _clearPool.Add(targetEntity);
                _clearPool.Add(hit);
                if (!_clearPool.Has(projectileEntity))
                {
                    _clearPool.Add(projectileEntity);
                }
            }
        }
    }
}