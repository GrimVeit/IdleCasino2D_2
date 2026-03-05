using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteSpotModel
{
    public void ActivateAnimation(string name)
    {
        OnActivateAnimation?.Invoke(name);
    }


    public void Click()
    {
        Debug.Log("Click");

        OnClick?.Invoke();
    }

    #region Output

    public event Action OnClick;

    public event Action<string> OnActivateAnimation;

    #endregion
}
