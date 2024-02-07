using _Project.Scripts.Ecs.Dependencies;
using UnityEngine;

namespace _Project.Ecs.Scripts.Core.Common.View
{
    public interface IViewFactory
    {
        TView Create<TView>(int entity, string name,Vector3 position, Quaternion rotation, Transform parent) where TView : class, IView;
        void Destroy(IView view);
    }
}