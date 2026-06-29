using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDealer : IStaff
{
    public void ActivateAnimation(DealerAnimationEnum animationEnum);

    //Interactive
    public event Action<IDealer> OnClick;

    //Message
    void SetMessage(string message, SpeechTurnEnum turn, bool isSound = false);
    void SetMessage(string message, bool isSound = false);
}
