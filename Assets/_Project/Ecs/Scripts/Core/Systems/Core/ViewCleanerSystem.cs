using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Common.View
{
    internal class ViewCleanerSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private readonly IViewFactory _viewFactory;
        
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<EcsGameObject> _ecsGameObjectPool;
        
        public ViewCleanerSystem(IViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<EcsGameObject>().Inc<Clear>().End();
            _ecsGameObjectPool = _world.GetPool<EcsGameObject>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var ecsGameObject = ref _ecsGameObjectPool.Get(entity);
                _viewFactory.Destroy(ecsGameObject.View);
                _ecsGameObjectPool.Del(entity);
            }
        }
    }
}