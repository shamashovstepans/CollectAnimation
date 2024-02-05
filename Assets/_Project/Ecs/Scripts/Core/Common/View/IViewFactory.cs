using _Project.Scripts.Ecs.Dependencies;
using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Common.View
{
    public interface IViewFactory
    {
        IView Create(int entity, string name,Vector3 position, Quaternion rotation, Transform parent);
        void Destroy(IView view);
    }
}