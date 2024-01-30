using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Scripting;

namespace Game.Ground
{
    internal class Ground : MonoBehaviour, IGround
    {
        [SerializeField] private float _borderPadding;
        
        [OnValueChanged("OnValueChanged")]
        [SerializeField] private float _width;
        [OnValueChanged("OnValueChanged")]
        [SerializeField] private float _lenght;
        [OnValueChanged("OnValueChanged")]
        [SerializeField] private float _height;

        [SerializeField] private Transform _groundTransform;
        [SerializeField] private Transform _leftBorderTransform;
        [SerializeField] private Transform _rightBorderTransform;
        [SerializeField] private Transform _topBorderTransform;
        [SerializeField] private Transform _bottomBorderTransform;

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

        public Vector3 GetClosestPoint(Vector3 position)
        {
            return _bounds.ClosestPoint(position);
        }
    }
}