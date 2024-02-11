using UnityEngine;

namespace _Project.Scripts.Ecs.View
{
    internal class ArrowRotationView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody = default;

        private Vector3 _lastPosition;

        private void FixedUpdate()
        {
            var velocity = _rigidbody.position - _lastPosition;
            if (velocity != Vector3.zero)
            {
                _rigidbody.MoveRotation(Quaternion.LookRotation(velocity));
                _lastPosition = _rigidbody.position;
            }
        }
    }
}