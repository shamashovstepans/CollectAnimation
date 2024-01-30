using UnityEngine;

namespace Game
{
    public interface IResource
    {
        Transform Anchor { get; }
        void MarkAsDetected();
        void Collect();
    }
}