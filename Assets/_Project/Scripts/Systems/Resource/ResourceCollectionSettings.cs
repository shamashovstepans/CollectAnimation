using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    internal class ResourceCollectionSettings : ScriptableObject
    {
        [SerializeField] private float _flyRadius;
        [SerializeField] private float _flySpeed;
        
        [SerializeField] private float _spawnRadius;
        [SerializeField] private float _spawnSpeed;
        
        public float FlyRadius => _flyRadius;
        public float FlySpeed => _flySpeed;
        
        public float SpawnRadius => _spawnRadius;
        public float SpawnSpeed => _spawnSpeed;
    }
}