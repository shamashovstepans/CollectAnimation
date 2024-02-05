using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class MovingStateSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<Standing> _standingPool;
        private EcsPool<Moving> _movingPool;
        private EcsPool<ObjectRigidbody> _objectRigidbodyPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<ObjectRigidbody>().End();
            _standingPool = _world.GetPool<Standing>();
            _movingPool = _world.GetPool<Moving>();
            _objectRigidbodyPool = _world.GetPool<ObjectRigidbody>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var objectRigidbody = ref _objectRigidbodyPool.Get(entity);
                if (objectRigidbody.Velocity.magnitude > 0)
                {
                    if (_standingPool.Has(entity))
                    {
                        _standingPool.Del(entity);
                    }

                    if (!_movingPool.Has(entity))
                    {
                        _movingPool.Add(entity);
                    }
                }
                else
                {
                    if (_movingPool.Has(entity))
                    {
                        _movingPool.Del(entity);
                    }

                    if (!_standingPool.Has(entity))
                    {
                        _standingPool.Add(entity);
                    }
                }
            }
        }
    }
}