using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    internal class ResourceCollectionTask : IDisposable
    {
        private readonly Transform _resource;
        private readonly Transform _target;
        private readonly ResourceCollectionSettings _settings;
        private readonly CancellationTokenSource _lifetimeToken;

        public ResourceCollectionTask(Transform resource, Transform target, ResourceCollectionSettings settings)
        {
            _resource = resource;
            _target = target;
            _settings = settings;
            _lifetimeToken = new CancellationTokenSource();
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var token = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _lifetimeToken.Token).Token;
            var resource = _resource;
            var target = _target;
            var settings = _settings;

            var startPosition = resource.position;
            var targetPosition = target.position;
            var distance = Vector3.Distance(startPosition, targetPosition);
            var direction = (targetPosition - startPosition).normalized;
            var flyRadius = settings.FlyRadius;
            var flySpeed = settings.FlySpeed;
            var time = distance / flySpeed;
            var timePassed = 0f;
            while (timePassed < time)
            {
                token.ThrowIfCancellationRequested();

                var t = timePassed / time;
                var position = Vector3.Lerp(startPosition, targetPosition, t);
                position += direction * Mathf.Sin(t * Mathf.PI) * flyRadius;
                resource.position = position;
                timePassed += Time.deltaTime;
                await Task.Yield();
            }
        }

        public void Dispose()
        {
            _lifetimeToken?.Cancel();
            _lifetimeToken?.Dispose();
        }
    }
}