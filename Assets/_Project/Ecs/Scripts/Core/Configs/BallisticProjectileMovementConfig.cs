using _Project.Scripts.Ecs.Core.Common;
using UnityEngine;

namespace _Project.Scripts.Ecs.Configs
{
    [CreateAssetMenu]
    internal class BallisticProjectileMovementConfig : EcsConfig
    {
        [SerializeField] private float _maxHeight = 3;
        [SerializeField] private float _speed = 10;

        public float MaxHeight => _maxHeight;
        public float Speed => _speed;
    }
}