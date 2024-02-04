using _Project.Scripts.Ecs.Core.Common;
using UnityEngine;

namespace _Project.Scripts.Ecs.Configs
{
    [CreateAssetMenu]
    public class FollowTargetConfig : EcsConfig
    {
        [SerializeField] private float _followSpeed = 1f;
        
        public float FollowSpeed => _followSpeed;
    }
}