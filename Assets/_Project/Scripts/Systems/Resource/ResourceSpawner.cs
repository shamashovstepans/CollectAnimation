using System;
using System.Collections.Generic;
using Game.Ground;
using Game.Ui;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game
{
    internal class ResourceSpawner : IInitializable, IDisposable, ITickable
    {
        private readonly IInputDetector _inputDetector;
        private readonly IGround _ground;
        private readonly ResourceCollectionSettings _settings;
        private readonly ResourcesParent _resourcesParent;

        private readonly List<Resource> _resources = new();

        private bool _isPointerDown;
        private Vector3 _pointerPosition;
        private float _timeSinceLastSpawn;

        public ResourceSpawner(
            IInputDetector inputDetector,
            ResourceCollectionSettings settings,
            IGround ground,
            ResourcesParent resourcesParent)
        {
            _inputDetector = inputDetector;
            _settings = settings;
            _ground = ground;
            _resourcesParent = resourcesParent;
        }

        public void Initialize()
        {
            _inputDetector.PointerDown += OnPointerDown;
            _inputDetector.PointerUp += OnPointerUp;
            _inputDetector.PointerDrag += OnPointerDrag;
        }

        public void Dispose()
        {
            _inputDetector.PointerDown -= OnPointerDown;
            _inputDetector.PointerUp -= OnPointerUp;

            foreach (var resource in _resources)
            {
                if (resource == null)
                {
                    continue;
                }
                resource.Collected -= OnResourceCollected;
                Object.Destroy(resource.gameObject);
            }

            _resources.Clear();
        }

        public void Tick()
        {
            _timeSinceLastSpawn += Time.deltaTime;

            if (_timeSinceLastSpawn < _settings.SpawnInterval)
            {
                return;
            }

            if (_isPointerDown)
            {
                var randomPosition = Random.insideUnitCircle * _settings.SpawnRadius;
                var spawnPosition = new Vector3(randomPosition.x, 0f, randomPosition.y) + _pointerPosition;

                if (!_ground.TryGetClosestPoint(spawnPosition, out var closestPoint))
                {
                    return;
                }

                SpawnResource(closestPoint);
                _timeSinceLastSpawn = 0f;
            }
            else
            {
                if (_resources.Count >= _settings.MaxResources)
                {
                    return;
                }

                var randomPosition = _ground.GetRandomPoint();
                SpawnResource(randomPosition);
                _timeSinceLastSpawn = 0f;
            }
        }

        private void SpawnResource(Vector3 position)
        {
            var positionOnGround = new Vector3(position.x, _settings.ResourcePrefab.transform.position.y, position.z);
            var resource = Object.Instantiate(_settings.ResourcePrefab, positionOnGround, Quaternion.identity, _resourcesParent.transform);
            _resources.Add(resource);
            resource.Collected += OnResourceCollected;
        }

        private void OnResourceCollected(Resource resource)
        {
            resource.Collected -= OnResourceCollected;
            _resources.Remove(resource);
            Object.Destroy(resource.gameObject);
        }

        private void OnPointerDown(Vector3 position)
        {
            _isPointerDown = true;
            _pointerPosition = position;
        }

        private void OnPointerDrag(Vector3 position)
        {
            _isPointerDown = true;
            _pointerPosition = position;
        }

        private void OnPointerUp(Vector3 position)
        {
            _isPointerDown = false;
            _pointerPosition = position;
        }
    }
}