using _Project.Scripts.Ecs.Core.Common;
using UnityEngine;

namespace _Project.Scripts.Ecs.Configs
{
    [CreateAssetMenu]
    public class RotationConfig : EcsConfig
    {
        [SerializeField] private float _rotationSpeed = 1f;
        
        public float RotationSpeed => _rotationSpeed;
    }
}