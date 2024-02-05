using _Project.Scripts.Ecs.Core.Common;
using UnityEngine;

namespace _Project.Scripts.Ecs.Configs
{
    public class PlayerConfig : EcsConfig
    {
        [SerializeField] private GameObject _playerPrefab = default;
        [SerializeField] private float _health = 100f;

        public GameObject PlayerPrefab => _playerPrefab;
        public float Health => _health;
    }
}