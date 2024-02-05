using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace _Project_legacy.Scripts.Papers
{
    public interface IHolder
    {
        int ItemsCount { get; }
        Task<IHoldable> TakeAsync(Transform parent, CancellationToken cancellationToken);
        Task PutAsync(IHoldable holdable, CancellationToken cancellationToken);
    }
}