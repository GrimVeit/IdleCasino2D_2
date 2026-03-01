using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongstressModel : IStaffModel
{
    public void SetAnimation(SongstressAnimationEnum animationEnum)
    {
        OnSetAnimation?.Invoke(animationEnum);
    }

    #region Output

    public event Action<SongstressAnimationEnum> OnSetAnimation;

    #endregion
}
