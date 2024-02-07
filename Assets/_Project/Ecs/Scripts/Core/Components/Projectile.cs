using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Components
{
    public struct Projectile
    {
        public Vector3 StartPosition;
        public int Target;
        public float Damage;
        public float Speed;
        public float Lifetime;
    }
}