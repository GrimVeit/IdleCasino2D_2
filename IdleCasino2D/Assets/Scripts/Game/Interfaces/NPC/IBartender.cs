using System;

public interface IBartender : IStaff
{
    public void ActivateAnimation(BartenderAnimationEnum animationEnum);

    //Interactive
    public event Action<IBartender> OnClick;

    //Message
    void SetMessage(string message, SpeechTurnEnum turn);
    void SetMessage(string message);
}
