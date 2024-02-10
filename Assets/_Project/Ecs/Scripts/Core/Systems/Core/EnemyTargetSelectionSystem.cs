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
        private EcsPool<FollowTarget> _followPool;
        private EcsPool<PhysicalBody> _physicalBodyPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _playerFilter = _world.Filter<PhysicalBody>().Inc<PlayerTag>().End();
            _enemyFilter = _world.Filter<FollowTarget>().Inc<EnemyTag>().End();
            _followPool = _world.GetPool<FollowTarget>();
            _physicalBodyPool = _world.GetPool<PhysicalBody>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var playerEntity in _playerFilter)
            {
                foreach (var enemyEntity in _enemyFilter)
                {
                    ref var followTarget = ref _followPool.Get(enemyEntity);
                    var physicalBody = _physicalBodyPool.Get(playerEntity);
                    followTarget.TargetPosition = physicalBody.Position;
                }
            }
        }
    }
}