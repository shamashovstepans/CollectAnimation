using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Configs;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class MovableRotationSystem : IEcsInitSystem, IEcsPhysicsRunSystem
    {
        private readonly RotationConfig _rotationConfig;

        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<ObjectRigidbody> _objectRigidbodyPool;

        public MovableRotationSystem(RotationConfig rotationConfig)
        {
            _rotationConfig = rotationConfig;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<ObjectRigidbody>().Inc<ObjectTransform>().Inc<Moving>().End();
            _objectRigidbodyPool = _world.GetPool<ObjectRigidbody>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var objectRigidbody = ref _objectRigidbodyPool.Get(entity);

                var velocity = objectRigidbody.Velocity;
                if (velocity.sqrMagnitude > 0)
                {
                    objectRigidbody.Rotation = Quaternion.Lerp(objectRigidbody.Rotation, Quaternion.LookRotation(velocity), Time.deltaTime * _rotationConfig.RotationSpeed);
                }
            }
        }
    }
}