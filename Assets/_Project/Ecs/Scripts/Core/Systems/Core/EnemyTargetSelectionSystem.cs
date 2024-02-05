using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Scripts.Ecs.Systems
{
    internal class EnemyTargetSelectionSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _playerFilter;
        private EcsFilter _enemyFilter;
        private EcsPool<ObjectTransform> _objectTransformPool;
        private EcsPool<FollowTarget> _followPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _playerFilter = _world.Filter<ObjectTransform>().Inc<PlayerTag>().End();
            _enemyFilter = _world.Filter<FollowTarget>().Inc<EnemyTag>().End();
            _objectTransformPool = _world.GetPool<ObjectTransform>();
            _followPool = _world.GetPool<FollowTarget>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var playerEntity in _playerFilter)
            {
                foreach (var enemyEntity in _enemyFilter)
                {
                    ref var followTarget = ref _followPool.Get(enemyEntity);
                    var playerTransform = _objectTransformPool.Get(playerEntity);
                    followTarget.TargetPosition = playerTransform.Position;
                }
            }
        }
    }
}