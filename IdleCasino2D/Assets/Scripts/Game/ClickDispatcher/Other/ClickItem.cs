using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickItem : MonoBehaviour, IClick
{
    public void Click()
    {
        Debug.Log("Click to - " + gameObject.name);

        OnClick?.Invoke();
    }

    #region Output

    public event Action OnClick;

    #endregion
}
