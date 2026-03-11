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

    #region Output

    public event Action<ManagerAnimationEnum> OnSetAnimation;

    #endregion
}
