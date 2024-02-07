using _Project.Scripts.Ecs.Dependencies;
using UnityEngine;

namespace _Project.Scripts.Ecs.View
{
    internal class EcsWorldView : MonoBehaviour, IEcsWorldView, IWorldRotation
    {
        [SerializeField] private Transform _worldTransform = default;
        [SerializeField] private Transform _enemiesParent = default;
        [SerializeField] private Transform _projectilesParent = default;
        [SerializeField] private EcsGroundView _groundView = default;

        public Transform Transform => transform;
        public Transform EnemiesParent => _enemiesParent;
        public Transform BulletsParent => _projectilesParent;
        public Quaternion Rotation => _worldTransform.rotation;

        public Vector3 GetRandomBorderPoint()
        {
            return _groundView.GetRandomBorderPoint();
        }
    }
}