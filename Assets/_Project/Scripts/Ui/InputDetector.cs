using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Ui
{
    internal class InputDetector : MonoBehaviour, IInputDetector, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public event Action<Vector3> PointerDown;
        public event Action<Vector3> PointerUp;
        public event Action<Vector3> PointerDrag;

        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private Camera _gameCamera;

        public void OnPointerDown(PointerEventData eventData)
        {
            var ray = _gameCamera.ScreenPointToRay(eventData.position);
            if (Physics.Raycast(ray, out var hit, float.MaxValue, _groundLayer))
            {
                PointerDown?.Invoke(hit.point);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            var ray = _gameCamera.ScreenPointToRay(eventData.position);
            if (Physics.Raycast(ray, out var hit, float.MaxValue, _groundLayer))
            {
                PointerUp?.Invoke(hit.point);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            var ray = _gameCamera.ScreenPointToRay(eventData.position);
            if (Physics.Raycast(ray, out var hit, float.MaxValue, _groundLayer))
            {
                PointerDrag?.Invoke(hit.point);
            }
        }
    }
}