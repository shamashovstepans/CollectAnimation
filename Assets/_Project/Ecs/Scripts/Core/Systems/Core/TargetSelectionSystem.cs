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
        private EcsPool<PhysicalBody> _physicalBodyPool;
        private EcsPool<TargetNotFound> _targetNotFoundPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _targetFindersFilter = _world.Filter<FindTarget>().Inc<PhysicalBody>().End();
            _enemiesFilter = _world.Filter<EnemyTag>().Inc<PhysicalBody>().End();
            _targetPool = _world.GetPool<Target>();
            _physicalBodyPool = _world.GetPool<PhysicalBody>();
            _findTargetPool = _world.GetPool<FindTarget>();
            _targetNotFoundPool = _world.GetPool<TargetNotFound>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var targetFinder in _targetFindersFilter)
            {
                var finderTransform = _physicalBodyPool.Get(targetFinder);
                var minDistance = float.MaxValue;
                var targetFound = false;

                foreach (var enemyEntity in _enemiesFilter)
                {
                    var enemyTransform = _physicalBodyPool.Get(enemyEntity);
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