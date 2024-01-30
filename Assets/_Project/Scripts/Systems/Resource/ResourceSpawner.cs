using System;
using Game.Ground;
using Game.Ui;
using UnityEngine;
using Zenject;

namespace Game
{
    internal class ResourceSpawner : IInitializable, IDisposable, ITickable
    {
        private readonly IInputDetector _inputDetector;
        private readonly IGround _ground;
        private readonly ResourceCollectionSettings _settings;

        private bool _isPointerDown;
        private Vector3 _pointerDownPosition;

        public ResourceSpawner(IInputDetector inputDetector, ResourceCollectionSettings settings)
        {
            _inputDetector = inputDetector;
            _settings = settings;
        }

        public void Initialize()
        {
            _inputDetector.PointerDown += OnPointerDown;
            _inputDetector.PointerUp += OnPointerUp;
        }

        public void Dispose()
        {
            _inputDetector.PointerDown -= OnPointerDown;
            _inputDetector.PointerUp -= OnPointerUp;
        }

        public void Tick()
        {

        }

        private void OnPointerDown(Vector3 position)
        {
            _isPointerDown = true;
        }

        private void OnPointerUp(Vector3 position)
        {
            _isPointerDown = false;
        }


    }
}