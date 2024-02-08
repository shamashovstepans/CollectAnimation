using _Project.Ecs.Scripts.Core.Components;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class TargetSelectionSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _targetFindersFilter;
        private EcsFilter _enemiesFilter;
        private EcsPool<Target> _targetPool;
        private EcsPool<FindTarget> _findTargetPool;
        private EcsPool<ObjectTransform> _objectTransformPool;
        private EcsPool<TargetNotFound> _targetNotFoundPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _targetFindersFilter = _world.Filter<FindTarget>().Inc<ObjectTransform>().End();
            _enemiesFilter = _world.Filter<EnemyTag>().Inc<ObjectTransform>().End();
            _targetPool = _world.GetPool<Target>();
            _objectTransformPool = _world.GetPool<ObjectTransform>();
            _findTargetPool = _world.GetPool<FindTarget>();
            _targetNotFoundPool = _world.GetPool<TargetNotFound>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var targetFinder in _targetFindersFilter)
            {
                var finderTransform = _objectTransformPool.Get(targetFinder);
                var minDistance = float.MaxValue;
                var targetFound = false;

                foreach (var enemyEntity in _enemiesFilter)
                {
                    var enemyTransform = _objectTransformPool.Get(enemyEntity);
                    var distance = (enemyTransform.Position - finderTransform.Position).sqrMagnitude;
                    if (distance < minDistance)
                    {
                        if (!_targetPool.Has(targetFinder))
                        {
                            _targetPool.Add(targetFinder);
                            _findTargetPool.Del(targetFinder);
                        }

                        targetFound = true;

                        minDistance = distance;
                        ref var target = ref _targetPool.Get(targetFinder);
                        target.TargetEntityPacked = _world.PackEntity(enemyEntity);
                    }
                }

                if (!targetFound)
                {
                    if (!_findTargetPool.Has(targetFinder))
                    {
                        _targetPool.Add(targetFinder);
                    }
                    if (!_targetNotFoundPool.Has(targetFinder))
                    {
                        _targetNotFoundPool.Add(targetFinder);
                    }
                }
            }
        }
    }
}