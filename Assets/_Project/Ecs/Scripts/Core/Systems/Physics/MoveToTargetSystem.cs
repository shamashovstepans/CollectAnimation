using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Configs;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Scripts.Ecs.Systems
{
    internal class MoveToTargetSystem : IEcsInitSystem, IEcsPhysicsRunSystem
    {
        private readonly FollowTargetConfig _config;

        private EcsWorld _world;
        private EcsFilter _followersFilter;
        private EcsPool<FollowTarget> _followPool;
        private EcsPool<ObjectRigidbody> _objectRigidbodyPool;
        private EcsPool<ObjectTransform> _objectTransformPool;

        public MoveToTargetSystem(FollowTargetConfig config)
        {
            _config = config;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _followersFilter = _world.Filter<FollowTarget>().Inc<ObjectRigidbody>().Inc<ObjectTransform>().End();
            _followPool = _world.GetPool<FollowTarget>();
            _objectRigidbodyPool = _world.GetPool<ObjectRigidbody>();
            _objectTransformPool = _world.GetPool<ObjectTransform>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var followerEntity in _followersFilter)
            {
                var followTarget = _followPool.Get(followerEntity);
                var objectTransform = _objectTransformPool.Get(followerEntity);
                ref var objectRigidbody = ref _objectRigidbodyPool.Get(followerEntity);

                var direction = followTarget.TargetPosition - objectTransform.Position;

                var velocity = direction.normalized * _config.FollowSpeed;
                objectRigidbody.Velocity = velocity;
            }
        }
    }
}