using UnityEngine;

namespace Game
{
    public interface IResource
    {
        Transform Anchor { get; }
        void MarkAsProcessed();
    }
}