using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Scripts.Ecs.Systems
{
    internal class UpdateRigidbodySystem : IEcsPhysicsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<ObjectRigidbody> _objectRigidbodyPool;
        private EcsPool<PhysicalBody> _physicalBodyPool;
        private EcsPool<ObjectTransform> _objectTransformPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<ObjectRigidbody>().Inc<PhysicalBody>().End();
            _objectRigidbodyPool = _world.GetPool<ObjectRigidbody>();
            _physicalBodyPool = _world.GetPool<PhysicalBody>();
            _objectTransformPool = _world.GetPool<ObjectTransform>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var objectRigidbody = _objectRigidbodyPool.Get(entity);
                var objectTransform = _objectTransformPool.Get(entity);
                ref var physicalBody = ref _physicalBodyPool.Get(entity);

                physicalBody.View.SetVelocity(objectRigidbody.Velocity);
                physicalBody.View.SetRotation(objectTransform.Rotation);
            }
        }
    }
}