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
        private EcsPool<PhysicalBody> _physicalBodyPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<PhysicalBody>().End();
            _standingPool = _world.GetPool<Standing>();
            _movingPool = _world.GetPool<Moving>();
            _physicalBodyPool = _world.GetPool<PhysicalBody>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var objectRigidbody = ref _physicalBodyPool.Get(entity);
                if (objectRigidbody.Velocity.magnitude > 0.1f)
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