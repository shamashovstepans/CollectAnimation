using UnityEngine;

namespace _Project.Scripts.Ecs.View
{
    internal class ArrowRotationView : MonoBehaviour 
    {
        [SerializeField] private Rigidbody _rigidbody = default;
        
        private void FixedUpdate()
        {
            if (_rigidbody.velocity != Vector3.zero)
            {
                _rigidbody.rotation = Quaternion.LookRotation(_rigidbody.velocity);
            }
        }
    }
}