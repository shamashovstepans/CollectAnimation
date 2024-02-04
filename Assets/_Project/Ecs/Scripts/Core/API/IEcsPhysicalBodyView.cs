using _Project.Scripts.Ecs.Components;
using UnityEngine;

namespace _Project.Scripts.Ecs.Dependencies
{
    public interface IEcsPhysicalBodyView
    {
        ObjectTransform GetTransform();
        void SetVelocity(Vector3 velocity);
    }
}