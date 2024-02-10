using _Project.Ecs.Scripts.Core.Components;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Configs;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Systems.Core
{
    internal class LegacyProjectileMovementSystem : IEcsInitSystem, IEcsPhysicsRunSystem
    {
        private readonly ShootingConfig _shootingConfig;
        private EcsWorld _world;
        private EcsFilter _projectileFilter;
        private EcsPool<Projectile> _projectilePool;
        private EcsPool<Target> _targetPool;

        public LegacyProjectileMovementSystem(ShootingConfig shootingConfig)
        {
            _shootingConfig = shootingConfig;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _projectileFilter = _world.Filter<Projectile>().Inc<Target>().Inc<PhysicalBody>().End();
            _projectilePool = _world.GetPool<Projectile>();
            _targetPool = _world.GetPool<Target>();
        }

        public void Run(IEcsSystems systems)
        {
            // foreach (var projectileEntity in _projectileFilter)
            // {
            //     ref var projectile = ref _projectilePool.Get(projectileEntity);
            //     ref var objectRigidbody = ref _objectRigidbodyPool.Get(projectileEntity);
            //     ref var objectTransform = ref _objectTransformPool.Get(projectileEntity);
            //     var target = _targetPool.Get(projectileEntity);
            //
            //     var targetEntityPacked = target.TargetEntityPacked;
            //
            //     if (!targetEntityPacked.Unpack(_world, out var targetEntity))
            //     {
            //         continue;
            //     }
            //
            //     var targetTransform = _objectTransformPool.Get(targetEntity);
            //     var startPosition = new Vector2(projectile.StartPosition.x, projectile.StartPosition.z);
            //     var endPosition = new Vector2(targetTransform.Position.x, targetTransform.Position.z);
            //     var currentPosition = new Vector2(objectTransform.Position.x, objectTransform.Position.z);
            //     var totalDistance = (endPosition - startPosition).magnitude;
            //     var distanceFromStart = (currentPosition - startPosition).magnitude;
            //     var progress = distanceFromStart / totalDistance;
            //
            //     progress = Mathf.Clamp01(progress + 0.1f);
            //
            //     var x = Mathf.Lerp(startPosition.x, endPosition.x, progress);
            //     var y = CalculateY(projectile.StartPosition.y, targetTransform.Position.y, progress, _parabolicFlightPool.Get(projectileEntity));
            //     var z = Mathf.Lerp(startPosition.y, endPosition.y, progress);
            //
            //     var calculatedPosition = new Vector3(x, y, z);
            //     // + _shootingConfig.BulletSpawnOffset
            //     var direction = (calculatedPosition - objectRigidbody.Position).normalized;
            //
            //     objectRigidbody.Velocity = direction * projectile.Speed;
            // }
        }
        //
        // private float CalculateY(float start,
        //     float end,
        //     float progress,
        //     ParabolicFlight parabolicFlight)
        // {
        //     var defaultY = Mathf.Lerp(start, end, progress);
        //
        //     var y = GetYPosition(progress, parabolicFlight.Height);
        //
        //     return defaultY + y;
        // }
        //
        // private float GetYPosition(float time, float height)
        // {
        //     var absToCenter = Mathf.Abs(time - 0.5f);
        //     absToCenter = Func(absToCenter);
        //     var relativeToCenter = (Func(0.5f) - absToCenter) / Func(0.5f);
        //     var absoluteHeight = relativeToCenter * height;
        //     return absoluteHeight;
        // }
        //
        // private float Func(float x)
        // {
        //     return x * x;
        // }
    }
}