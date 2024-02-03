using _Project.Scripts.Ecs.Components;
using _Project.Scripts.Ecs.Dependencies;
using UnityEngine;

namespace _Project.Scripts.Ecs.View
{
    internal class EcsPhysicalBodyView : MonoBehaviour, IEcsPhysicalBodyView
    {
        [SerializeField] private Transform _transform = default;
        [SerializeField] private Rigidbody _rigidbody = default;

        public ObjectTransform GetTransform()
        {
            return new ObjectTransform
            {
                Position = _transform.position,
                Rotation = _transform.rotation
            };
        }

        public void Move(Vector3 deltaMovement)
        {
            _rigidbody.MovePosition(_rigidbody.position + deltaMovement);
        }
    }
}