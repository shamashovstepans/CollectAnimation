using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace _Project_legacy.Scripts.Papers
{
    internal class PlayerHands : MonoBehaviour
    {
        private IHolder _holder;

        private readonly Stack<IHoldable> _holdables = new();

        private CancellationTokenSource _lifetimeTokenSource;
        private Task _actionTask;

        private bool _isTaking;

        private void OnEnable()
        {
            _lifetimeTokenSource = new CancellationTokenSource();
        }

        private void OnDisable()
        {
            _lifetimeTokenSource.Cancel();
            _lifetimeTokenSource.Dispose();
        }

        private void Update()
        {
            if (_holder == null)
            {
                return;
            }

            if (_isTaking)
            {
                Take();
            }
            else
            {
                Put();
            }
        }

        private void Take()
        {
            if (_holder.ItemsCount == 0)
            {
                return;
            }

            var isTaskCompleted = _actionTask?.IsCompleted ?? true;

            if (!isTaskCompleted)
            {
                return;
            }

            _actionTask = TakeAsync();
        }

        private async Task TakeAsync()
        {
            var takeTask = _holder.TakeAsync(transform, _lifetimeTokenSource.Token);
            var holdable = await takeTask;
            _holdables.Push(holdable);
        }

        private void Put()
        {
            if (_holdables.Count == 0)
            {
                return;
            }

            var isTaskCompleted = _actionTask?.IsCompleted ?? true;

            if (!isTaskCompleted)
            {
                return;
            }

            _actionTask = PutAsync();
        }

        private Task PutAsync()
        {
            var holdable = _holdables.Pop();
            return _holder.PutAsync(holdable, _lifetimeTokenSource.Token);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IHolder holder))
            {
                _holder = holder;

                _isTaking = _holdables.Count == 0;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IHolder holder))
            {
                if (_holder == holder)
                {
                    _holder = null;
                }
            }
        }
    }
}