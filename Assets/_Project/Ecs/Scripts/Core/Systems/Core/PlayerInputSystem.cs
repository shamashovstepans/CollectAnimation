using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Scripts.Ecs.Systems
{
    internal class PlayerInputSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<PlayerInput> _playerInputPool;

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<PlayerInput>().End();
            _playerInputPool = _world.GetPool<PlayerInput>();
        }

        public void Run(EcsSystems systems)
        {
            var horizontal = UnityEngine.Input.GetAxisRaw("Horizontal");
            var vertical = UnityEngine.Input.GetAxisRaw("Vertical");
            
            var movement = new UnityEngine.Vector2(horizontal, vertical);
            
            foreach (var entity in _filter)
            {
                ref var playerInput = ref _playerInputPool.Get(entity);
                playerInput.Movement = movement;
            }
        }
    }
}