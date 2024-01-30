using UnityEngine;

namespace Game
{
    internal class Resource : MonoBehaviour, IResource
    {
        public Transform Anchor => transform;

        public void MarkAsProcessed()
        {
        }
    }
}