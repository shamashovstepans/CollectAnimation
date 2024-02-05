using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Scripting;

namespace _Project.Scripts.Ecs.View
{
    internal class EcsGroundView : MonoBehaviour
    {
        [SerializeField] private float _borderPadding = 2f;

        [OnValueChanged("OnValueChanged")]
        [SerializeField] private float _width = 20f;
        [OnValueChanged("OnValueChanged")]
        [SerializeField] private float _lenght = 20f;
        [OnValueChanged("OnValueChanged")]
        [SerializeField] private float _height = 5;

        [SerializeField] private Transform _groundTransform = default;
        [SerializeField] private Transform _leftBorderTransform = default;
        [SerializeField] private Transform _rightBorderTransform = default;
        [SerializeField] private Transform _topBorderTransform = default;
        [SerializeField] private Transform _bottomBorderTransform = default;

        private Bounds _bounds;

        [Preserve]
        private void OnValueChanged()
        {
            _groundTransform.localScale = new Vector3(_width, 1f, _lenght);

            _leftBorderTransform.localPosition = new Vector3(-_width / 2f, _height / 2f, 0f);
            _rightBorderTransform.localPosition = new Vector3(_width / 2f, _height / 2f, 0f);
            _topBorderTransform.localPosition = new Vector3(0f, _height / 2f, _lenght / 2f);
            _bottomBorderTransform.localPosition = new Vector3(0f, _height / 2f, -_lenght / 2f);

            _leftBorderTransform.localScale = new Vector3(1f, _height, _lenght);
            _rightBorderTransform.localScale = new Vector3(1f, _height, _lenght);
            _topBorderTransform.localScale = new Vector3(_width, _height, 1f);
            _bottomBorderTransform.localScale = new Vector3(_width, _height, 1f);

            _bounds = new Bounds(_groundTransform.position, new Vector3(_width - _borderPadding, _height, _lenght - _borderPadding));
        }

        private void OnEnable()
        {
            OnValueChanged();
        }

        public Vector3 GetRandomBorderPoint()
        {
            var randomPoint = new Vector3(
                Random.Range(-_bounds.size.x / 2f, _bounds.size.x / 2f),
                0f,
                Random.Range(-_bounds.size.z / 2f, _bounds.size.z / 2f)
            );

            var randomBorder = Random.Range(0, 4);
            switch (randomBorder)
            {
                case 0:
                    randomPoint.x = -_bounds.size.x / 2f;
                    break;
                case 1:
                    randomPoint.x = _bounds.size.x / 2f;
                    break;
                case 2:
                    randomPoint.z = -_bounds.size.z / 2f;
                    break;
                case 3:
                    randomPoint.z = _bounds.size.z / 2f;
                    break;
            }

            return randomPoint;
        }
    }
}