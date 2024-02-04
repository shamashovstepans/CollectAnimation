using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Core.Common;
using _Project.Scripts.Ecs.Dependencies;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Scripts.Ecs.Systems
{
    internal class PlayerInputSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private readonly IWorldRotation _worldRotation;
        
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<PlayerInput> _playerInputPool;
        
        public PlayerInputSystem(IWorldRotation worldRotation)
        {
            _worldRotation = worldRotation;
        }

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<PlayerInput>().End();
            _playerInputPool = _world.GetPool<PlayerInput>();
        }

        public void Run(EcsSystems systems)
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            
            var input = new Vector3(horizontal, 0f, vertical);
            var localDirection = Quaternion.Inverse(_worldRotation.Rotation) * input;
            var directionClamped = Vector3.ClampMagnitude(localDirection, 1f);
            
            foreach (var entity in _filter)
            {
                ref var playerInput = ref _playerInputPool.Get(entity);
                playerInput.MovementInput = new Vector2(directionClamped.x, directionClamped.z);
            }
        }
    }
}