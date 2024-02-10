using UnityEngine;

namespace _Project.Scripts.Ecs.Dependencies
{
    public interface IEcsPhysicalBodyView : IView
    {
        Transform Transform { get; }
        Rigidbody Rigidbody { get; }
    }
}