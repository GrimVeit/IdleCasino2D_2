using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MotionBase : MonoBehaviour
{
    public virtual void Initialize() { }
    public virtual void Dispose() { }
    public abstract void Activate();
    public abstract void Deactivate();
}
