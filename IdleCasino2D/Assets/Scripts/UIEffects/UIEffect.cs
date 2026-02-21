using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIEffect : MonoBehaviour
{
    public bool IsActive => isActive;
    private protected bool isActive;

    public virtual void Initialize() { }
    public virtual void Dispose() { }
    public virtual void ActivateEffect(Action OnComplete = null) { }
    public virtual void DeactivateEffect(Action OnComplete = null) { }
    public virtual void ResetEffect() { }
}
