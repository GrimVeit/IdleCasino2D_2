using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BartenderModel : IStaffModel
{
    public void SetAnimation(BartenderAnimationEnum animationEnum)
    {
        OnSetAnimation?.Invoke(animationEnum);
    }

    public void Click()
    {
        OnClick?.Invoke();
    }

    #region Output

    public event Action<BartenderAnimationEnum> OnSetAnimation;
    public event Action OnClick;

    #endregion
}
