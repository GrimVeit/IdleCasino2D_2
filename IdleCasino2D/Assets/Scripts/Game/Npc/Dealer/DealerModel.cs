using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerModel
{
    public void SetIdle()
    {
        OnSetIdle?.Invoke();
    }

    public void SetPlay()
    {
        OnSetPlay?.Invoke();
    }

    #region Output

    public event Action OnSetIdle;
    public event Action OnSetPlay;

    #endregion
}
