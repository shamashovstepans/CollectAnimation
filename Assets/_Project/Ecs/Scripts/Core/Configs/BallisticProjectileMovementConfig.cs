using _Project.Scripts.Ecs.Core.Common;
using UnityEngine;

namespace _Project.Scripts.Ecs.Configs
{
    [CreateAssetMenu]
    internal class BallisticProjectileMovementConfig : EcsConfig
    {
        [SerializeField] private float _maxHeight = 3;
        
        public float MaxHeight => _maxHeight;
    }
}