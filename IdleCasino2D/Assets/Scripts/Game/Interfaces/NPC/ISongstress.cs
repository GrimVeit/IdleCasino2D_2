using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISongstress : IStaff
{
    public void ActivateAnimation(SongstressAnimationEnum animationEnum);

    //Interactive
    public event Action<ISongstress> OnClick;

    //Message
    void SetMessage(string message, SpeechTurnEnum turn);
    void SetMessage(string message);
}
