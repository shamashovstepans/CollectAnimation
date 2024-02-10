using _Project.Ecs.Scripts.Core.Components;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Systems.Physics
{
    internal class BallisticProjectileMovementSystem : IEcsInitSystem, IEcsPhysicsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<Projectile> _projectilePool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<Projectile>().Inc<PhysicalBody>().End();
            _projectilePool = _world.GetPool<Projectile>();
        }

        public void Run(IEcsSystems systems)
        {
        }
    }
}