using _Project.Scripts.Ecs.Core.Common;
using UnityEngine;

namespace _Project.Scripts.Ecs.Configs
{
    [CreateAssetMenu]
    public class ShootingConfig : EcsConfig
    {
        [Range(0, 100f)]
        [SerializeField] private float _damage = 50f;
        [Range(0.05f, 3f)]
        [SerializeField] private float _cooldown = 1f;
        [SerializeField] private Vector3 _bulletSpawnOffset;
        [SerializeField] private float _launchAngle;

        public float Damage => _damage;
        public float Cooldown => _cooldown;
        public Vector3 BulletSpawnOffset => _bulletSpawnOffset;
        public float LaunchAngle => _launchAngle;
    }
}