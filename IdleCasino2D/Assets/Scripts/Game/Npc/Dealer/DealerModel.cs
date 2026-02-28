using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerModel : IStaffModel
{
    public void SetAnimation(DealerAnimationEnum animationEnum)
    {
        OnSetAnimation?.Invoke(animationEnum);
    }

    #region Output

    public event Action<DealerAnimationEnum> OnSetAnimation;

    #endregion
}
