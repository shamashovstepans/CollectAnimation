using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Project_legacy.Scripts.Papers
{
    internal class Table : MonoBehaviour, IHolder
    {
        [SerializeField] private Transform _holdablesRoot;
        [SerializeField] private float _timeToTake = 0.5f;

        private readonly Stack<IHoldable> _items = new();

        public int ItemsCount => _items.Count;

        private void OnEnable()
        {
            var items = _holdablesRoot.GetComponentsInChildren<IHoldable>();
            foreach (var item in items)
            {
                _items.Push(item);
            }
        }

        public async Task<IHoldable> TakeAsync(Transform parent, CancellationToken cancellationToken)
        {
            if (ItemsCount == 0)
                throw new Exception("No items to take");

            var holdable = _items.Pop();
            holdable.Transform.SetParent(parent, true);
            await holdable.Transform.DOLocalMove(Vector3.up, _timeToTake).PlayAsync(cancellationToken);
            return holdable;
        }

        public async Task PutAsync(IHoldable holdable, CancellationToken cancellationToken)
        {
            holdable.Transform.SetParent(_holdablesRoot, true);
            await holdable.Transform.DOLocalMove(Vector3.zero, _timeToTake).PlayAsync(cancellationToken);
            _items.Push(holdable);
        }
    }
}