using _Project.Ecs.Scripts.Core.Components;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class PlayerFindTargetSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _playerWithoutTarget;
        private EcsPool<FindTarget> _findTargetPool;
        private EcsPool<TargetNotFound> _targetNotFoundPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _playerWithoutTarget = _world.Filter<PlayerTag>().Inc<Standing>().Exc<Target>().Inc<TargetNotFound>().End();
            _findTargetPool = _world.GetPool<FindTarget>();
            _targetNotFoundPool = _world.GetPool<TargetNotFound>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _playerWithoutTarget)
            {
                _targetNotFoundPool.Del(entity);
            }
        }
    }
}