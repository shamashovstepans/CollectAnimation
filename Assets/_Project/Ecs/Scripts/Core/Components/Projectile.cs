using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Components
{
    public struct Projectile
    {
        public int SpawnerEntity;
        public Vector3 StartPosition;
        public int Target;
        public float Damage;
        public float Speed;
        public float Lifetime;
    }
}