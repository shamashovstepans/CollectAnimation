using UnityEngine;

namespace _Project.Scripts.Ecs.Dependencies
{
    public interface IEcsWorldView
    {
        Transform Transform { get; }
    }
}