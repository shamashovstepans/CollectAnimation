using UnityEngine;

namespace Game.Ground
{
    public interface IGround
    {
        Vector3 GetClosestPoint(Vector3 position);
    }
}