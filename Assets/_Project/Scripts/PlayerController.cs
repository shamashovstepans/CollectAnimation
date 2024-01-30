using Game.Ground;
using UnityEngine;
using Zenject;

namespace Game
{
    internal class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _worldRotation;
        [SerializeField] private PlayerSettings _settings;

        [Inject] private readonly IGround _ground;

        private void Update()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var direction = new Vector3(horizontal, 0f, vertical);

            var localDirection = Quaternion.Inverse(_worldRotation.rotation) * direction;
            var directionClamped = Vector3.ClampMagnitude(localDirection, 1f);

            var newPosition = transform.position + directionClamped * (_settings.Speed * Time.deltaTime);
            
            transform.position = _ground.GetClosestPoint(newPosition);
        }
    }
}