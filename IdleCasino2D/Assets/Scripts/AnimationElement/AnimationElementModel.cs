using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationElementModel
{
    public void Activate(string id, int cycles)
    {
        OnActivateAnimation?.Invoke(id, cycles);
    }

    public void Deactivate(string id)
    {
        OnDeactivateAnimation?.Invoke(id);
    }

    #region Output

    public event Action<string, int> OnActivateAnimation;
    public event Action<string> OnDeactivateAnimation;

    #endregion
}
