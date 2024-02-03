using UnityEngine;

namespace Game.Ground
{
    public interface IGround
    {
        bool TryGetClosestPoint(Vector3 position, out Vector3 closestPoint);
        Vector3 GetRandomPoint();
        Vector3 GetRandomBorderPoint();
    }
}