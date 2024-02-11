using Leopotam.EcsLite;
using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Components
{
    public struct Projectile
    {
        public float FlightTime;
        public Vector3 StartPosition;
        public Vector3 EndPosition;
        public EcsPackedEntity SpawnerEntity;
        public float Damage;
    }
}