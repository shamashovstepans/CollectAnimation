using _Project.Scripts.Ecs.Core.Common;
using UnityEngine;

namespace _Project.Scripts.Ecs.Configs
{
    public class PlayerMovementConfig : EcsConfig
    {
        [SerializeField] private float _speed = 5;
        
        public float Speed => _speed;
    }
}