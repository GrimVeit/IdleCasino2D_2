using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager : IStaff
{
    public void ActivateAnimation(ManagerAnimationEnum animationEnum);

    //Interactive
    public event Action<IManager> OnClick;

    //Message
    void SetMessage(string message, SpeechTurnEnum turn);
    void SetMessage(string message);
}
