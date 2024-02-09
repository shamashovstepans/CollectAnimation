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
                var shootingEntityPacked = _world.PackEntity(shootingEntity);
                var targetEntityPacked = _targetPool.Get(shootingEntity).TargetEntityPacked;

                if (!targetEntityPacked.Unpack(_world, out var targetEntity))
                {
                    continue;
                }

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
                projectile.SpawnerEntity = shootingEntityPacked;
                projectile.Damage = _config.Damage;

                var targetPhysicalBody = _objectRigidbodyPool.Get(targetEntity);

                var startPosition = shootingTransform.Position + _config.BulletSpawnOffset;

                // ref var projectileRigidbody = ref _objectRigidbodyPool.Add(projectileEntity);
                var velocity = GetLaunchVelocity(startPosition, targetPhysicalBody.Position, _config.LaunchAngle);
                Debug.Log(velocity);
                // projectileRigidbody.Position = startPosition;
                // projectileRigidbody.Velocity = velocity;

                var projectileView = _viewFactory.Create<IEcsPhysicalBodyView>(projectileEntity, ViewConst.Projectile, startPosition, Quaternion.identity, _worldView.BulletsParent);

                ref var projectilePhysicalBody = ref _physicalBodyPool.Add(projectileEntity);
                projectilePhysicalBody.View = projectileView;
                projectilePhysicalBody.View.SetVelocity(velocity);
            }
        }

        private Vector3 GetLaunchVelocity(Vector3 startPosition, Vector3 endPosition, float launchAngle)
        {
            var projectileXZPos = new Vector3(startPosition.x, 0.0f, startPosition.z);
            var targetXZPos = new Vector3(endPosition.x, 0.0f, endPosition.z);
            var rotation = Quaternion.LookRotation(targetXZPos - projectileXZPos);

            // shorthands for the formula
            var R = Vector3.Distance(projectileXZPos, targetXZPos);
            var G = UnityEngine.Physics.gravity.y;
            var tanAlpha = Mathf.Tan(launchAngle * Mathf.Deg2Rad);
            var H = endPosition.y - startPosition.y;

            // calculate the local space components of the velocity 
            // required to land the projectile on the target object 
            var Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
            var Vy = tanAlpha * Vz;

            // create the velocity vector in local space and get it in global space
            var velocity = rotation * new Vector3(0f, Vy, Vz);

            return velocity;
        }
    }
}