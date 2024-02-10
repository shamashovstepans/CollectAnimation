using _Project.Ecs.Scripts.Core.Common.View;
using _Project.Ecs.Scripts.Core.Components;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Configs;
using _Project.Scripts.Ecs.Dependencies;
using _Project.Scripts.Ecs.View;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Scripts.Ecs.Systems
{
    internal class PlayerInitializationSystem : IEcsInitSystem
    {
        private readonly PlayerConfig _playerConfig;
        private readonly IEcsWorldView _worldView;
        private readonly IViewFactory _viewFactory;

        private EcsWorld _world;
        private EcsPool<PhysicalBody> _physicalBodyPool;
        private EcsPool<PlayerInput> _playerInputPool;
        private EcsPool<PlayerTag> _playerTagPool;
        private EcsPool<Health> _healthPool;
        private EcsPool<FindTarget> _findTargetPool;

        public PlayerInitializationSystem(
            PlayerConfig playerConfig,
            IEcsWorldView worldView,
            IViewFactory viewFactory)
        {
            _worldView = worldView;
            _viewFactory = viewFactory;
            _playerConfig = playerConfig;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _physicalBodyPool = _world.GetPool<PhysicalBody>();
            _playerInputPool = _world.GetPool<PlayerInput>();
            _playerTagPool = _world.GetPool<PlayerTag>();
            _healthPool = _world.GetPool<Health>();
            _findTargetPool = _world.GetPool<FindTarget>();

            var entity = _world.NewEntity();

            ref var physicalBody = ref _physicalBodyPool.Add(entity);
            var playerView = _viewFactory.Create<IEcsPhysicalBodyView>(entity, ViewConst.Player, Vector3.zero, Quaternion.identity, _worldView.Transform);
            physicalBody.View = playerView;

            _playerInputPool.Add(entity);
            _playerTagPool.Add(entity);
            _findTargetPool.Add(entity);

            ref var health = ref _healthPool.Add(entity);
            health.Value = _playerConfig.Health;
        }
    }
}