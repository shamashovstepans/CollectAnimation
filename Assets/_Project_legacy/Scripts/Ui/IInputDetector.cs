using System;
using UnityEngine;

namespace Game.Ui
{
    public interface IInputDetector
    {
        event Action<Vector3> PointerDown;
        event Action<Vector3> PointerUp;
        event Action<Vector3> PointerDrag;
    }
}