using UnityEngine;

namespace _Project.Scripts.Ecs.Dependencies
{
    public interface IEcsWorldView
    {
        Transform Transform { get; }
        Transform EnemiesParent { get; }
        Transform BulletsParent { get; }
        Vector3 GetRandomBorderPoint();
    }
}