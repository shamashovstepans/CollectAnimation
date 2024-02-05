using System;
using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Dependencies;
using UnityEngine;

namespace _Project.Scripts.Ecs.View
{
    internal class EcsPhysicalBodyView : MonoBehaviour, IEcsPhysicalBodyView
    {
        [SerializeField] private Transform _transform = default;
        [SerializeField] private Rigidbody _rigidbody = default;

        public Guid Id { get; } = Guid.NewGuid();

        public ObjectTransform GetTransform()
        {
            return new ObjectTransform
            {
                Position = _transform.position,
                Rotation = _transform.rotation
            };
        }

        public void SetVelocity(Vector3 velocity)
        {
            _rigidbody.velocity = velocity;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        public void SetRotation(Quaternion rotation)
        {
            _rigidbody.rotation = rotation.normalized;
        }
    }
}