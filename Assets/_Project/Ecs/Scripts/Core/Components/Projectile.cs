using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Components
{
    public struct Projectile
    {
        public Vector3 StartPosition;
        public EcsPackedEntity SpawnerEntity;
        public float Damage;
    }
}