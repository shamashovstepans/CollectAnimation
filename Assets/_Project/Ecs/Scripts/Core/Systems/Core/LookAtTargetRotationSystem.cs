using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Configs;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class LookAtTargetRotationSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private readonly RotationConfig _rotationConfig;

        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<ObjectTransform> _objectTransformPool;
        private EcsPool<Target> _targetPool;

        public LookAtTargetRotationSystem(RotationConfig rotationConfig)
        {
            _rotationConfig = rotationConfig;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<ObjectTransform>().Inc<Target>().Inc<Standing>().End();
            _objectTransformPool = _world.GetPool<ObjectTransform>();
            _targetPool = _world.GetPool<Target>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {

                ref var objectTransform = ref _objectTransformPool.Get(entity);
                var target = _targetPool.Get(entity);
                if (target.TargetEntity < 0)
                    continue;
                var targetTransform = _objectTransformPool.Get(target.TargetEntity);

                var direction = targetTransform.Position - objectTransform.Position;
                if (direction != Vector3.zero)
                {
                    objectTransform.Rotation = Quaternion.Lerp(objectTransform.Rotation, Quaternion.LookRotation(direction), Time.deltaTime * _rotationConfig.RotationSpeed);
                }
            }
        }
    }
}