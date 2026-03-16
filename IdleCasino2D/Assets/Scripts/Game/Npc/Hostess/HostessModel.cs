using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostessModel : IStaffModel
{
    public void SetAnimation(HostessAnimationEnum animationEnum)
    {
        OnSetAnimation?.Invoke(animationEnum);
    }

    public void Click()
    {
        OnClick?.Invoke();
    }

    #region Output

    public event Action<HostessAnimationEnum> OnSetAnimation;
    public event Action OnClick;

    #endregion
}
