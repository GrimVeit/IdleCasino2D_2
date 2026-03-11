using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager : IStaff
{
    public void ActivateAnimation(ManagerAnimationEnum animationEnum);
}
