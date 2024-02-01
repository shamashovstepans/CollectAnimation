using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{
    internal class ResourceCollector : MonoBehaviour, IResourceCollector
    {
        public event Action<IResourceCollector, IResource> ResourceDetected;

        [SerializeField] private Transform _controlPointsParent;
        [SerializeField] private Transform _controlPointPrefab;
        [SerializeField] private Transform _target;

        [SerializeField] private ResourceCollectionSettings _settings;

        [SerializeField] private UnityEvent _onResourceCollected;

        [Inject] private readonly IResourceCollectionSystem _resourceCollectionSystem;

        public Transform Anchor => transform;
        public Transform RandomControlPoint => _controlPoints[Random.Range(0, _controlPoints.Count)];

        private readonly List<Transform> _controlPoints = new();

        private void OnEnable()
        {
            _resourceCollectionSystem.Register(this);
            _settings.Updated += OnSettingsUpdated;
            OnSettingsUpdated();
        }

        private void OnDisable()
        {
            _resourceCollectionSystem.Remove(this);
            _settings.Updated -= OnSettingsUpdated;
        }

        private void Update()
        {
            RecalculatePositions();
        }
        
        public void NotifyCollected(IResource resource)
        {
            _onResourceCollected?.Invoke();
        }

        private void RecalculatePositions()
        {
            var controlPointsCount = _controlPoints.Count;
            var maxAngle = _settings.MaxAngle;
            var radius = _settings.Radius;
            var height = _settings.Height;
            
            var angleStep = maxAngle / controlPointsCount;
            var angle = -maxAngle / 2f + _target.rotation.eulerAngles.y - 180f;

            for (var i = 0; i < controlPointsCount; i++)
            {
                angle += angleStep;
                var radians = angle * Mathf.Deg2Rad;
                var x = Mathf.Sin(radians) * radius;
                var z = Mathf.Cos(radians) * radius;
                var direction = new Vector3(x, height, z);
                _controlPoints[i].localPosition = Vector3.Slerp(_controlPoints[i].localPosition, _target.position + direction, _settings.ControlPointFollowSpeed * Time.deltaTime);
            }
        }

        private void OnSettingsUpdated()
        {
            foreach (var controlPoint in _controlPoints)
            {
                Destroy(controlPoint.gameObject);
            }

            _controlPoints.Clear();

            for (var i = 0; i < _settings.ControlPointsCount; i++)
            {
                var controlPoint = Instantiate(_controlPointPrefab, _controlPointsParent);
                controlPoint.localPosition = Vector3.zero;
                _controlPoints.Add(controlPoint);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IResource resource))
            {
                ResourceDetected?.Invoke(this, resource);
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var controlPoint in _controlPoints)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(controlPoint.position, 0.2f);
            }
        }
    }
}