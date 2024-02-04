using UnityEngine;

namespace _Project.Scripts.Ecs.Dependencies
{
    public interface IWorldRotation
    {
        Quaternion Rotation { get; }
    }
}