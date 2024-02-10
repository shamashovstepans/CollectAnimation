using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Configs;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class LookAtTargetRotationSystem : IEcsInitSystem, IEcsPhysicsRunSystem
    {
        private readonly RotationConfig _rotationConfig;

        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<PhysicalBody> _physicalBodyPool;
        private EcsPool<Target> _targetPool;

        public LookAtTargetRotationSystem(RotationConfig rotationConfig)
        {
            _rotationConfig = rotationConfig;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<PhysicalBody>().Inc<Target>().Inc<Standing>().End();
            _targetPool = _world.GetPool<Target>();
            _physicalBodyPool = _world.GetPool<PhysicalBody>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var objectRigidbody = ref _physicalBodyPool.Get(entity);
                var target = _targetPool.Get(entity);

                if (!target.TargetEntityPacked.Unpack(_world, out var targetEntity))
                {
                    continue;
                }

                var targetTransform = _physicalBodyPool.Get(targetEntity);

                var direction = targetTransform.Position - objectRigidbody.Position;
                if (direction != Vector3.zero)
                {
                    objectRigidbody.Rotation = Quaternion.Lerp(objectRigidbody.Rotation, Quaternion.LookRotation(direction), Time.deltaTime * _rotationConfig.RotationSpeed);
                }
            }
        }
    }
}