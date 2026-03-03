using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHostess : IStaff
{
    public void ActivateAnimation(HostessAnimationEnum animationEnum);
}
