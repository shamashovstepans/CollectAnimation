using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    internal class ResourceCollectionSettings : ScriptableObject
    {
        [SerializeField] private float _flyRadius;
        [SerializeField] private float _flySpeed = 5f;
        
        [SerializeField] private float _spawnRadius;
        [SerializeField] private float _spawnInterval;

        [SerializeField] private Resource _resourcePrefab;
        
        public float FlyRadius => _flyRadius;
        public float FlySpeed => _flySpeed;
        
        public float SpawnRadius => _spawnRadius;
        public float SpawnInterval => _spawnInterval;
        
        public Resource ResourcePrefab => _resourcePrefab;
    }
}