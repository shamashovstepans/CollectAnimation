using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class LookAtTargetRotationSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<LookAtTarget> _lookAtTargetPool;
        private EcsPool<ObjectTransform> _objectTransformPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<LookAtTarget>().Inc<ObjectTransform>().End();
            _lookAtTargetPool = _world.GetPool<LookAtTarget>();
            _objectTransformPool = _world.GetPool<ObjectTransform>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var lookAtTarget = ref _lookAtTargetPool.Get(entity);
                ref var objectTransform = ref _objectTransformPool.Get(entity);

                var direction = lookAtTarget.TargetPosition - objectTransform.Position;
                if (direction != Vector3.zero)
                {
                    objectTransform.Rotation = Quaternion.LookRotation(direction);
                }
            }
        }
    }
}