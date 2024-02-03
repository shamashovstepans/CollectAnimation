using UnityEngine;

namespace Game.Systems.Enemy
{
    [CreateAssetMenu]
    internal class EnemySpawnerSettings : ScriptableObject
    {
        [SerializeField] private Enemy _enemyPrefab = default;
        [SerializeField] private float _spawnInterval = 0.5f;
        
        [SerializeField] private float _enemySpeed = 5f;

        public float SpawnInterval => _spawnInterval;
        public Enemy EnemyPrefab => _enemyPrefab;
        public float EnemySpeed => _enemySpeed;
    }
}