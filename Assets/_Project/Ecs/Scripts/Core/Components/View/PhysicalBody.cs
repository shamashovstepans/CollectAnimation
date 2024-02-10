using _Project.Scripts.Ecs.Dependencies;
using UnityEngine;

namespace _Project.Scripts.Ecs.Components
{
    public struct PhysicalBody
    {
        public IEcsPhysicalBodyView View;

        public Vector3 Position
        {
            get => View.Rigidbody.position;
            set => View.Rigidbody.position = value;
        }

        public Quaternion Rotation
        {
            get => View.Rigidbody.rotation;
            set => View.Rigidbody.rotation = value;
        }

        public Vector3 Velocity
        {
            get => View.Rigidbody.velocity;
            set => View.Rigidbody.velocity = value;
        }
    }
}