using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Configs;
using _Project.Scripts.Ecs.Dependencies;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Scripts.Ecs.Systems
{
    internal class PlayerInitializationSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private readonly PlayerConfig _playerConfig;
        private readonly IEcsWorldView _worldView;
        
        private EcsWorld _world;
        private EcsPool<ObjectRigidbody> _objectRigidbodyPool;
        private EcsPool<PhysicalBody> _physicalBodyPool;
        private EcsPool<PlayerInput> _playerInputPool;
        private EcsPool<ObjectTransform> _objectTransformPool;
        private EcsPool<PlayerTag> _playerTagPool;

        private IEcsPhysicalBodyView _playerView;
        
        private GameObject _playerObject;

        public PlayerInitializationSystem(
            PlayerConfig playerConfig,
            IEcsWorldView worldView)
        {
            _worldView = worldView;
            _playerConfig = playerConfig;
        }

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _objectRigidbodyPool = _world.GetPool<ObjectRigidbody>();
            _physicalBodyPool = _world.GetPool<PhysicalBody>();
            _playerInputPool = _world.GetPool<PlayerInput>();
            _objectTransformPool = _world.GetPool<ObjectTransform>();
            _playerTagPool = _world.GetPool<PlayerTag>();
            
            var entity = _world.NewEntity();

            ref var physicalBody = ref _physicalBodyPool.Add(entity);
            var playerView = CreatePlayer();
            physicalBody.View = playerView;
            
            _objectTransformPool.Add(entity);
            _objectRigidbodyPool.Add(entity);
            _playerInputPool.Add(entity);
            _playerTagPool.Add(entity);
        }

        public void Destroy(EcsSystems systems)
        {
            Object.Destroy(_playerObject);
            _playerObject = null;
        }
        
        private IEcsPhysicalBodyView CreatePlayer()
        {
            var prefab = _playerConfig.PlayerPrefab;
            _playerObject = Object.Instantiate(prefab, _worldView.Transform);
            return _playerObject.GetComponent<IEcsPhysicalBodyView>();
        }
    }
}