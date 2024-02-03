using _Project.Scripts.Ecs.Dependencies;
using UnityEngine;

namespace _Project.Scripts.Ecs.View
{
    internal class EcsWorldView : MonoBehaviour, IEcsWorldView
    {
        public Transform Transform => transform;
    }
}