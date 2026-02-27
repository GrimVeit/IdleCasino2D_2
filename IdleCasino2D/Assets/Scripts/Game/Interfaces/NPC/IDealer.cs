using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDealer : INpc
{
    public void ActivateAnimation(DealerAnimationEnum animationEnum);
}
