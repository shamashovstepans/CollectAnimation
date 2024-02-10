using _Project.Ecs.Scripts.Core.Components;
using _Project.Scripts.Ecs.Core.Common;
using _Project.Scripts.Ecs.Dependencies;
using LeoEcsPhysics;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Systems.Physics
{
    internal class CollisionProcessSystem : IEcsInitSystem, IEcsPhysicsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _collisionFilter;
        private EcsPool<OnTriggerEnterEvent> _onTriggerEnterPool;
        private EcsPool<ProjectileHit> _projectileHitPool;
        private EcsPool<Projectile> _projectilePool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _collisionFilter = _world.Filter<OnTriggerEnterEvent>().End();
            _onTriggerEnterPool = _world.GetPool<OnTriggerEnterEvent>();
            _projectileHitPool = _world.GetPool<ProjectileHit>();
            _projectilePool = _world.GetPool<Projectile>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var collisionEntity in _collisionFilter)
            {
                var collisionEvent = _onTriggerEnterPool.Get(collisionEntity);
                
                if (collisionEvent.collider == null || collisionEvent.senderGameObject == null)
                {
                    continue;
                }
                
                var targetEntity = collisionEvent.collider.gameObject.GetComponentInParent<IView>();
                var senderEntity = collisionEvent.senderGameObject.GetComponentInParent<IView>();

                if (targetEntity == null || senderEntity == null)
                {
                    continue;
                }

                var projectile = _projectilePool.Get(senderEntity.EntityId);

                if (!projectile.SpawnerEntity.Unpack(_world, out var spawnerEntity))
                {
                    continue;
                }

                if (spawnerEntity == targetEntity.EntityId)
                {
                    continue;
                }

                var projectileHitEntity = _world.NewEntity();
                ref var projectileHit = ref _projectileHitPool.Add(projectileHitEntity);
                projectileHit.TargetEntity = _world.PackEntity(targetEntity.EntityId);
                projectileHit.ProjectileEntity = _world.PackEntity(senderEntity.EntityId);
                
                collisionEvent.senderGameObject.gameObject.SetActive(false);
            }
        }
    }
}