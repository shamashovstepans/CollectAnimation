using _Project.Ecs.Scripts.Core.Components;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class PlayerShootingSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _playerStandingFilter;
        private EcsFilter _notStandingPlayerFilter;
        private EcsPool<Shooting> _shootingTagPool;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _playerStandingFilter = _world.Filter<PlayerTag>().Inc<Standing>().End();
            _notStandingPlayerFilter = _world.Filter<PlayerTag>().Exc<Standing>().End();
            _shootingTagPool = _world.GetPool<Shooting>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _playerStandingFilter)
            {
                if (!_shootingTagPool.Has(entity))
                {
                    _shootingTagPool.Add(entity);
                }
            }
            
            foreach (var entity in _notStandingPlayerFilter)
            {
                if (_shootingTagPool.Has(entity))
                {
                    _shootingTagPool.Del(entity);
                }
            }
        }
    }
}