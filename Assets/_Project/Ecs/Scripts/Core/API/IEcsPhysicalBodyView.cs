using _Project.Scripts.Ecs.Components;
using UnityEngine;

namespace _Project.Scripts.Ecs.Dependencies
{
    public interface IEcsPhysicalBodyView : IView
    {
        ObjectTransform GetTransform();
        ObjectRigidbody GetRigidbody();
        void SetVelocity(Vector3 velocity);
        void SetRotation(Quaternion rotation);
    }
}