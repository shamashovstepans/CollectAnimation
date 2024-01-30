using System;
using UnityEngine;
using Zenject;

namespace Game
{
    internal class ResourceCollector : MonoBehaviour, IResourceCollector
    {
        public event Action<IResourceCollector, IResource> ResourceDetected;
        
        [Inject] private readonly IResourceCollectionSystem _resourceCollectionSystem;

        public Transform Anchor => transform;
        
        private void OnEnable()
        {
            _resourceCollectionSystem.Register(this);
        }
        
        private void OnDisable()
        {
            _resourceCollectionSystem.Remove(this);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IResource resource))
            {
                ResourceDetected?.Invoke(this, resource);
            }
        }
    }
}