using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Configs;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Scripts.Ecs.Systems
{
    internal class PlayerMovementSystem : IEcsPhysicsRunSystem, IEcsInitSystem
    {
        private readonly PlayerMovementConfig _config;

        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<PlayerInput> _playerInputPool;
        private EcsPool<ObjectRigidbody> _objectRigidbodyPool;

        public PlayerMovementSystem(PlayerMovementConfig config)
        {
            _config = config;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<PlayerInput>().Inc<ObjectRigidbody>().End();
            _playerInputPool = _world.GetPool<PlayerInput>();
            _objectRigidbodyPool = _world.GetPool<ObjectRigidbody>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var playerInput = _playerInputPool.Get(entity);
                ref var objectRigidbody = ref _objectRigidbodyPool.Get(entity);

                var worldInput = new Vector3(playerInput.MovementInput.x, 0, playerInput.MovementInput.y);
                var deltaMovement = worldInput  * _config.Speed;
                objectRigidbody.Velocity = deltaMovement;
            }
        }
    }
}