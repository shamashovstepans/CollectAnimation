using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Scripts.Ecs.Systems
{
    internal class SyncTransformsSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<ObjectTransform> _objectTransformPool;
        private EcsPool<PhysicalBody> _physicalBodyPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<ObjectTransform>().Inc<PhysicalBody>().End();
            _objectTransformPool = _world.GetPool<ObjectTransform>();
            _physicalBodyPool = _world.GetPool<PhysicalBody>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var objectTransform = ref _objectTransformPool.Get(entity);
                var physicalBody = _physicalBodyPool.Get(entity);

                objectTransform = physicalBody.View.GetTransform();
            }
        }
    }
}