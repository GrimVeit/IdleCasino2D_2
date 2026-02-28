using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDealer : IStaff
{
    public void ActivateAnimation(DealerAnimationEnum animationEnum);
}
