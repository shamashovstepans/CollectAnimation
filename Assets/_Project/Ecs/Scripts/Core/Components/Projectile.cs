using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Components
{
    public struct Projectile
    {
        public EcsPackedEntity SpawnerEntity;
        public Vector3 StartPosition;
        public float Damage;
        public float Speed;
        public float Lifetime;
    }
}