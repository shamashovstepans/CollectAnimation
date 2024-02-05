using _Project.Scripts.Ecs.Core.Common;
using UnityEngine;

namespace _Project.Scripts.Ecs.Configs
{
    public class EnemySpawnerConfig : EcsConfig
    {
        [SerializeField] private float _spawnInterval = 0.5f;
        [SerializeField] private GameObject _enemyPrefab = default;
        [SerializeField] private float _health = 100f;
        
        public float SpawnInterval => _spawnInterval;
        public GameObject EnemyPrefab => _enemyPrefab;
        public float Health => _health;
    }
}