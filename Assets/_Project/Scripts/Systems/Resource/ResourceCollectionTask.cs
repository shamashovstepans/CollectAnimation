using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    internal class ResourceCollectionTask : IDisposable
    {
        private readonly Transform _resource;
        private readonly Vector3 _startPoint;
        private readonly Transform _target;
        private readonly Transform _controlPoint;
        private readonly ResourceCollectionSettings _settings;
        private readonly CancellationTokenSource _lifetimeToken;

        public ResourceCollectionTask(Transform resourcePosition,
            Transform target,
            Transform controlPoint,
            ResourceCollectionSettings settings)
        {
            _resource = resourcePosition;
            _startPoint = resourcePosition.position;
            _target = target;
            _controlPoint = controlPoint;
            _settings = settings;
            _lifetimeToken = new CancellationTokenSource();
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var token = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _lifetimeToken.Token).Token;

            var timePassed = 0f;

            var curveLength = CalculateCurveLength();

            while (timePassed < 1f)
            {
                token.ThrowIfCancellationRequested();

                var distanceToMove = _settings.FlySpeed * Time.deltaTime;
                timePassed += GetTIncrement(timePassed, distanceToMove, curveLength);
                timePassed = Mathf.Clamp01(timePassed); // Ensure t stays between 0 and 1
                var position = MoveAlongCurve(timePassed);
                _resource.position = position;

                await Task.Yield();
            }
        }

        public void Dispose()
        {
            _lifetimeToken?.Cancel();
            _lifetimeToken?.Dispose();
        }

        private float CalculateCurveLength()
        {
            var length = 0.0f;
            var prevPoint = _startPoint;
            var segments = 100;

            for (var i = 1; i <= segments; i++)
            {
                var t = i / (float)segments;
                var point = CalculateBezierPoint(t, _startPoint, _controlPoint.position, _target.position);
                length += Vector3.Distance(prevPoint, point);
                prevPoint = point;
            }

            return length;
        }

        private float GetTIncrement(float currentT, float distanceToMove, float curveLength)
        {
            return (distanceToMove / curveLength) / (1 - currentT);
        }

        private Vector3 MoveAlongCurve(float t)
        {
            return CalculateBezierPoint(t, _startPoint, _controlPoint.position, _target.position);
        }

        private Vector3 CalculateBezierPoint(float t,
            Vector3 p0,
            Vector3 p1,
            Vector3 p2)
        {
            // Quadratic Bezier curve formula
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 p = uu * p0; //first term
            p += 2 * u * t * p1; //second term
            p += tt * p2; //third term

            return p;
        }
    }
}