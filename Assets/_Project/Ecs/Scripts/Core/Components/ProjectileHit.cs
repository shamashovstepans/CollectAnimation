using Leopotam.EcsLite;

namespace _Project.Ecs.Scripts.Core.Components
{
    public struct ProjectileHit
    {
        public EcsPackedEntity TargetEntity;
        public EcsPackedEntity ProjectileEntity;
    }
}