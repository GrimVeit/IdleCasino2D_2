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
    void SetMessage(string message, SpeechTurnEnum turn);
    void SetMessage(string message);
}
