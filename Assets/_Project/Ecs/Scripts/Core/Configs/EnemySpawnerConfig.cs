using _Project.Scripts.Ecs.Core.Common;
using UnityEngine;

namespace _Project.Scripts.Ecs.Configs
{
    public class EnemySpawnerConfig : EcsConfig
    {
        [SerializeField] private float _spawnInterval;
        [SerializeField] private GameObject _enemyPrefab;
        
        public float SpawnInterval => _spawnInterval;
        public GameObject EnemyPrefab => _enemyPrefab;
    }
}