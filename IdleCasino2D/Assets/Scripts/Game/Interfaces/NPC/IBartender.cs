using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBartender : IStaff
{
    public void ActivateAnimation(BartenderAnimationEnum animationEnum);
}
