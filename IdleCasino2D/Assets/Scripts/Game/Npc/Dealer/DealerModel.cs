using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerModel : IStaffModel
{
    private ISoundProvider _soundProvider;

    public void SetAnimation(DealerAnimationEnum animationEnum)
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

    public event Action<DealerAnimationEnum> OnSetAnimation;
    public event Action OnClick;
    public event Action<string, SpeechTurnEnum> OnSetMessage;

    #endregion
}
