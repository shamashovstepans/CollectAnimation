using _Project.Ecs.Scripts.Core.Components;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Configs;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class ProjectileMovementSystem : IEcsInitSystem, IEcsPhysicsRunSystem
    {
        private readonly ShootingConfig _shootingConfig;
        private EcsWorld _world;
        private EcsFilter _projectileFilter;
        private EcsPool<Projectile> _projectilePool;
        private EcsPool<ObjectRigidbody> _objectRigidbodyPool;
        private EcsPool<ObjectTransform> _objectTransformPool;

        public ProjectileMovementSystem(ShootingConfig shootingConfig)
        {
            _shootingConfig = shootingConfig;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _projectileFilter = _world.Filter<Projectile>().Inc<ObjectRigidbody>().Inc<ObjectTransform>().End();
            _projectilePool = _world.GetPool<Projectile>();
            _objectRigidbodyPool = _world.GetPool<ObjectRigidbody>();
            _objectTransformPool = _world.GetPool<ObjectTransform>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var projectileEntity in _projectileFilter)
            {
                ref var projectile = ref _projectilePool.Get(projectileEntity);
                ref var objectRigidbody = ref _objectRigidbodyPool.Get(projectileEntity);
                ref var objectTransform = ref _objectTransformPool.Get(projectileEntity);

                var targetEntity = projectile.Target;
                var targetTransform = _objectTransformPool.Get(targetEntity);

                var direction = (targetTransform.Position + _shootingConfig.BulletSpawnOffset - objectTransform.Position).normalized;

                objectRigidbody.Velocity = direction * projectile.Speed;
            }
        }
    }
}