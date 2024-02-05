using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class PlayerTargetSelectionSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _playerFilter;
        private EcsFilter _enemiesFilter;
        private EcsPool<LookAtTarget> _lookAtTargetPool;
        private EcsPool<ObjectTransform> _objectTransformPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _playerFilter = _world.Filter<PlayerTag>().Inc<ObjectTransform>().Inc<Standing>().End();
            _enemiesFilter = _world.Filter<EnemyTag>().Inc<ObjectTransform>().End();
            _lookAtTargetPool = _world.GetPool<LookAtTarget>();
            _objectTransformPool = _world.GetPool<ObjectTransform>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var playerEntity in _playerFilter)
            {
                var playerTransform = _objectTransformPool.Get(playerEntity);
                var minDistance = float.MaxValue;
                Vector3 targetPosition = default;
                var hasTarget = false;

                foreach (var enemyEntity in _enemiesFilter)
                {
                    var enemyTransform = _objectTransformPool.Get(enemyEntity);
                    var distance = (enemyTransform.Position - playerTransform.Position).sqrMagnitude;
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        targetPosition = enemyTransform.Position;
                        hasTarget = true;
                    }
                }
                
                if (hasTarget)
                {
                    if (!_lookAtTargetPool.Has(playerEntity))
                    {
                        _lookAtTargetPool.Add(playerEntity).TargetPosition = targetPosition;
                    }
                    else
                    {
                        ref var lookAtTarget = ref _lookAtTargetPool.Get(playerEntity);
                        lookAtTarget.TargetPosition = targetPosition;
                    }
                }
                else
                {
                    if (_lookAtTargetPool.Has(playerEntity))
                    {
                        _lookAtTargetPool.Del(playerEntity);
                    }
                }
            }
        }
    }
}