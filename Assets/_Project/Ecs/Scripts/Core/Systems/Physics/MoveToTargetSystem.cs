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
        private EcsPool<PhysicalBody> _objectRigidbodyPool;

        public MoveToTargetSystem(FollowTargetConfig config)
        {
            _config = config;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _followersFilter = _world.Filter<FollowTarget>().Inc<PhysicalBody>().End();
            _followPool = _world.GetPool<FollowTarget>();
            _objectRigidbodyPool = _world.GetPool<PhysicalBody>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var followerEntity in _followersFilter)
            {
                var followTarget = _followPool.Get(followerEntity);
                var objectTransform = _objectRigidbodyPool.Get(followerEntity);
                ref var objectRigidbody = ref _objectRigidbodyPool.Get(followerEntity);

                var direction = followTarget.TargetPosition - objectTransform.Position;

                var velocity = direction.normalized * _config.FollowSpeed;
                objectRigidbody.Velocity = velocity;
            }
        }
    }
}