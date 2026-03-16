using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerModel : IStaffModel
{
    public void SetAnimation(ManagerAnimationEnum animationEnum)
    {
        OnSetAnimation?.Invoke(animationEnum);
    }

    public void Click()
    {
        OnClick?.Invoke();
    }

    #region Output

    public event Action<ManagerAnimationEnum> OnSetAnimation;
    public event Action OnClick;

    #endregion
}
