using _Project.Ecs.Scripts.Core.Components;
using _Project.Ecs.Scripts.Core.Systems.Core;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Configs;
using _Project.Scripts.Ecs.Core.Common;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Systems.Physics
{
    internal class BallisticProjectileMovementSystem : IEcsInitSystem, IEcsPhysicsRunSystem
    {
        private readonly BallisticProjectileMovementConfig _config;

        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<Projectile> _projectilePool;
        private EcsPool<Target> _targetPool;

        public BallisticProjectileMovementSystem(BallisticProjectileMovementConfig config)
        {
            _config = config;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<Projectile>().Inc<Target>().Inc<PhysicalBody>().End();
            _projectilePool = _world.GetPool<Projectile>();
            _targetPool = _world.GetPool<Target>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var projectile = ref _projectilePool.Get(entity);
                var target = _targetPool.Get(entity).TargetEntityPacked;

                projectile.FlightTime += Time.fixedDeltaTime;

                if (target.Unpack(_world, out var targetEntity))
                {
                    ref var targetPhysicalBody = ref _world.GetPool<PhysicalBody>().Get(targetEntity);
                    projectile.EndPosition = targetPhysicalBody.Position + new Vector3(0, 0.5f, 0);
                }

                ref var projectilePhysicalBody = ref _world.GetPool<PhysicalBody>().Get(entity);

                var position = GetPosition(projectile.FlightTime, projectile.StartPosition, projectile.EndPosition, _config.MaxHeight);
                projectilePhysicalBody.View.Rigidbody.MovePosition(position);
            }
        }

        private Vector3 GetPosition(float time,
            Vector3 start,
            Vector3 end,
            float maxHeight)
        {
            var direction = end - start;
            var groundDirection = new Vector3(direction.x, 0, direction.z);
            var distance = groundDirection.magnitude;
            var deltaPos = new Vector2(distance, direction.y);
            CalculatePathWithHeight(deltaPos, maxHeight, out var v0, out var angle);

            var x = v0 * time * Mathf.Cos(angle);
            var y = v0 * time * Mathf.Sin(angle) - (1f / 2f) * -UnityEngine.Physics.gravity.y * Mathf.Pow(time, 2);
            var directionNormalized = direction.normalized;
            var result = start + directionNormalized * x + Vector3.up * y;
            return result;
        }

        private void CalculatePathWithHeight(
            Vector2 deltaPos,
            float h,
            out float v0,
            out float angle)
        {
            var xt = deltaPos.x;
            var yt = deltaPos.y;
            var g = -UnityEngine.Physics.gravity.y;
            var b = Mathf.Sqrt(2 * g * h);
            var a = (-0.5f * g);
            var c = -yt;
            var maxTime = QuadraticEquation(a, b, c, 1);
            var minTime = QuadraticEquation(a, b, c, -1);
            var time = maxTime > minTime ? maxTime : minTime;
            angle = Mathf.Atan(b * time / xt);
            v0 = b / Mathf.Sin(angle);
        }

        private float QuadraticEquation(
            float a,
            float b,
            float c,
            float sign)
        {
            return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
        }
    }
}