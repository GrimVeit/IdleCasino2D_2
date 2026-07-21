using System;

public class TutorialDialogueModel
{
    public void SetMessage(string id, float duration)
    {
        OnSetMessage?.Invoke(id, duration);
    }

    #region Output

    public event Action<string, float> OnSetMessage;

    #endregion
}
