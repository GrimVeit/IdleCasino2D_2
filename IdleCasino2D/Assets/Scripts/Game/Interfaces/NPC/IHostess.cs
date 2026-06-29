using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHostess : IStaff
{
    public void ActivateAnimation(HostessAnimationEnum animationEnum);

    //Interactive
    public event Action<IHostess> OnClick;

    //Message
    void SetMessage(string message, SpeechTurnEnum turn, bool isSound = false);
    void SetMessage(string message, bool isSound = false);
}
