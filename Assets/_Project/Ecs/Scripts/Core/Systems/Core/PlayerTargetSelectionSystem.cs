using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class PlayerTargetSelectionSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _playerFilter;
        private EcsFilter _enemiesFilter;
        private EcsPool<Target> _targetPool;
        private EcsPool<ObjectTransform> _objectTransformPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _playerFilter = _world.Filter<PlayerTag>().Inc<ObjectTransform>().End();
            _enemiesFilter = _world.Filter<EnemyTag>().Inc<ObjectTransform>().End();
            _targetPool = _world.GetPool<Target>();
            _objectTransformPool = _world.GetPool<ObjectTransform>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var playerEntity in _playerFilter)
            {
                var playerTransform = _objectTransformPool.Get(playerEntity);
                ref var target = ref _targetPool.Get(playerEntity);
                var minDistance = float.MaxValue;
                int targetEntity = -1;

                foreach (var enemyEntity in _enemiesFilter)
                {
                    var enemyTransform = _objectTransformPool.Get(enemyEntity);
                    var distance = (enemyTransform.Position - playerTransform.Position).sqrMagnitude;
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        targetEntity = enemyEntity;
                    }
                }

                target.TargetEntity = targetEntity;
            }
        }
    }
}