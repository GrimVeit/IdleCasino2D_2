using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerModel : IStaffModel
{
    private ISoundProvider _soundProvider;

    public void SetAnimation(ManagerAnimationEnum animationEnum)
    {
        OnSetAnimation?.Invoke(animationEnum);
    }

    public void Click()
    {
        OnClick?.Invoke();
    }

    public void SetSoundProvider(ISoundProvider soundProvider)
    {
        _soundProvider = soundProvider;
    }

    public void SetMessage(string message, SpeechTurnEnum turnEnum)
    {
        _soundProvider?.PlayOneShot("Message");

        OnSetMessage?.Invoke(message, turnEnum);
    }

    #region Output

    public event Action<ManagerAnimationEnum> OnSetAnimation;
    public event Action OnClick;
    public event Action<string, SpeechTurnEnum> OnSetMessage;

    #endregion
}
