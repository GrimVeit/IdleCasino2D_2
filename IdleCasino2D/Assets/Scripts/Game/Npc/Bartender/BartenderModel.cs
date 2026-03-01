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

    #region Output

    public event Action<BartenderAnimationEnum> OnSetAnimation;

    #endregion
}
