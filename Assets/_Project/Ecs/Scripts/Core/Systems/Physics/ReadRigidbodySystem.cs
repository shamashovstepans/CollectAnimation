using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Systems.Physics
{
    internal class ReadRigidbodySystem : IEcsInitSystem, IEcsPhysicsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<ObjectRigidbody> _objectRigidbodyPool;
        private EcsPool<PhysicalBody> _physicalBodyPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<ObjectRigidbody>().Inc<PhysicalBody>().End();
            _objectRigidbodyPool = _world.GetPool<ObjectRigidbody>();
            _physicalBodyPool = _world.GetPool<PhysicalBody>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var objectRigidbody = ref _objectRigidbodyPool.Get(entity);
                ref var physicalBody = ref _physicalBodyPool.Get(entity);

                objectRigidbody = physicalBody.View.GetRigidbody();
            }
        }
    }
}