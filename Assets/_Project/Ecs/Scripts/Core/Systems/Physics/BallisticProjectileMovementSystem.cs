using _Project.Ecs.Scripts.Core.Components;
using _Project.Ecs.Scripts.Core.Systems.Core;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Configs;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Systems.Physics
{
    internal class BallisticProjectileMovementSystem : IEcsInitSystem, IEcsPhysicsRunSystem
    {
        private readonly BallisticProjectileMovementConfig _config;
        
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<Projectile> _projectilePool;
        private EcsPool<Target> _targetPool;

        public BallisticProjectileMovementSystem(BallisticProjectileMovementConfig config)
        {
            _config = config;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<Projectile>().Inc<Target>().Inc<PhysicalBody>().End();
            _projectilePool = _world.GetPool<Projectile>();
            _targetPool = _world.GetPool<Target>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var projectile = ref _projectilePool.Get(entity);
                var target = _targetPool.Get(entity).TargetEntityPacked;
                
                if (!target.Unpack(_world, out var targetEntity))
                {
                    continue;
                }

                ref var targetPhysicalBody = ref _world.GetPool<PhysicalBody>().Get(targetEntity);
                ref var projectilePhysicalBody = ref _world.GetPool<PhysicalBody>().Get(entity);

                var targetPosition = targetPhysicalBody.Position + new Vector3(0, 1f, 0);

                var direction = (targetPosition - projectilePhysicalBody.Position).normalized;
                var velocity = direction * _config.Speed;
                projectilePhysicalBody.Velocity = velocity;
            }
        }
    }
}