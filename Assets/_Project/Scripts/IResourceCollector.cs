using System;
using UnityEngine;

namespace Game
{
    public interface IResourceCollector
    {
        event Action<IResourceCollector, IResource> ResourceDetected; 
        Transform Anchor { get; }
    }
}