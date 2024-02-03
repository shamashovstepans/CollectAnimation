using _Project.Scripts.Ecs.Core.Common;
using UnityEngine;

namespace _Project.Scripts.Ecs.Configs
{
    [CreateAssetMenu]
    public class PlayerConfig : EcsConfig
    {
        [SerializeField] private GameObject _playerPrefab;

        public GameObject PlayerPrefab => _playerPrefab;
    }
}