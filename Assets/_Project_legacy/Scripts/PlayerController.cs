using Game.Ground;
using Game.Systems.Enemy;
using UnityEngine;
using Zenject;

namespace Game
{
    internal class PlayerController : MonoBehaviour, IPlayer
    {
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");

        [SerializeField] private Transform _worldRotation;
        [SerializeField] private PlayerSettings _settings;
        [SerializeField] private Animator _animator;

        [Inject] private readonly IGround _ground;

        public Transform Transform => transform;

        private void Update()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var direction = new Vector3(horizontal, 0f, vertical);

            var localDirection = Quaternion.Inverse(_worldRotation.rotation) * direction;
            var directionClamped = Vector3.ClampMagnitude(localDirection, 1f);

            var isMoving = !Mathf.Approximately(directionClamped.magnitude, 0f);
            _animator.SetBool(IsRunning, isMoving);

            var newPosition = transform.position + directionClamped * (_settings.Speed * Time.deltaTime);

            _ground.TryGetClosestPoint(newPosition, out var closestPoint);
            transform.position = closestPoint;
            if (directionClamped != Vector3.zero)
            {
                transform.localRotation = Quaternion.LookRotation(directionClamped, Vector3.up);
            }
        }
    }
}