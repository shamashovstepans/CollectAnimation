using System;
using UnityEngine;
using Zenject;

namespace Game
{
    internal class Resource : MonoBehaviour, IResource
    {
        public event Action<Resource> Collected;
        
        [SerializeField] private Collider _collider;

        public Transform Anchor => transform;

        public void MarkAsDetected()
        {
            _collider.enabled = false;
        }

        public void Collect()
        {
            Collected?.Invoke(this);
        }
    }
}