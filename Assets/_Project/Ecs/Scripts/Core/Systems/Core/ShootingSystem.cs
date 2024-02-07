using _Project.Ecs.Scripts.Core.Common.View;
using _Project.Ecs.Scripts.Core.Components;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Configs;
using _Project.Scripts.Ecs.Core.Common;
using _Project.Scripts.Ecs.Dependencies;
using _Project.Scripts.Ecs.View;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class ShootingSystem : IEcsInitSystem, IEcsCoreRunSystem
    {
        private readonly ShootingConfig _config;
        private readonly IViewFactory _viewFactory;
        private readonly IEcsWorldView _worldView;

        private EcsWorld _world;
        private EcsFilter _shootingFilter;
        private EcsPool<Shooting> _shootingPool;
        private EcsPool<ObjectTransform> _objectTransformPool;
        private EcsPool<ObjectRigidbody> _objectRigidbodyPool;
        private EcsPool<Projectile> _projectilePool;
        private EcsPool<PhysicalBody> _physicalBodyPool;
        private EcsPool<Target> _targetPool;

        public ShootingSystem(ShootingConfig config, IViewFactory viewFactory, IEcsWorldView worldView)
        {
            _config = config;
            _viewFactory = viewFactory;
            _worldView = worldView;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _shootingFilter = _world.Filter<Shooting>().Inc<ObjectTransform>().Inc<Target>().End();
            _shootingPool = _world.GetPool<Shooting>();
            _objectTransformPool = _world.GetPool<ObjectTransform>();
            _objectRigidbodyPool = _world.GetPool<ObjectRigidbody>();
            _projectilePool = _world.GetPool<Projectile>();
            _physicalBodyPool = _world.GetPool<PhysicalBody>();
            _targetPool = _world.GetPool<Target>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var shootingEntity in _shootingFilter)
            {
                var targetEntity = _targetPool.Get(shootingEntity).TargetEntity;
                if (targetEntity < 0)
                {
                    continue;
                }
                ref var shooting = ref _shootingPool.Get(shootingEntity);
                
                shooting.CooldownTimer -= Time.deltaTime;

                if (shooting.CooldownTimer > 0)
                {
                    continue;
                }

                shooting.CooldownTimer = _config.Cooldown;

                var shootingTransform = _objectTransformPool.Get(shootingEntity);

                var projectileEntity = _world.NewEntity();

                ref var projectile = ref _projectilePool.Add(projectileEntity);
                projectile.Damage = _config.Damage;
                projectile.Speed = _config.Speed;
                projectile.Lifetime = _config.Lifetime;
                projectile.StartPosition = shootingTransform.Position;
                projectile.Target = targetEntity;

                ref var projectileTransform = ref _objectTransformPool.Add(projectileEntity);
                projectileTransform.Position = _objectTransformPool.Get(shootingEntity).Position;

                _objectRigidbodyPool.Add(projectileEntity);

                var projectileView = _viewFactory.Create<IEcsPhysicalBodyView>(projectileEntity, ViewConst.Projectile, shootingTransform.Position + _config.BulletSpawnOffset, Quaternion.identity, _worldView.BulletsParent);

                ref var projectilePhysicalBody = ref _physicalBodyPool.Add(projectileEntity);
                projectilePhysicalBody.View = projectileView;
            }
        }
    }
}