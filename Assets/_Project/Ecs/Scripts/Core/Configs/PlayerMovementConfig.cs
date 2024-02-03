using _Project.Scripts.Ecs.Core.Common;
using UnityEngine;

namespace _Project.Scripts.Ecs.Configs
{
    [CreateAssetMenu]
    public class PlayerMovementConfig : EcsConfig
    {
        [SerializeField] private float _speed;
        
        public float Speed => _speed;
    }
}