using System;
using _Project.Scripts.Ecs.Dependencies;
using UnityEngine;

namespace _Project.Scripts.Ecs.View
{
    internal class EcsPhysicalBodyView : MonoBehaviour, IEcsPhysicalBodyView
    {
        [SerializeField] private Transform _transform = default;
        [SerializeField] private Rigidbody _rigidbody = default;

        public Guid Id { get; } = Guid.NewGuid();
        public int EntityId { get; set; }
        public Transform Transform => _transform;
        public Rigidbody Rigidbody => _rigidbody;
    }
}