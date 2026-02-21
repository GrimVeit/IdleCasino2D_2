using System;
public class WheelSpotModel
{
    public void ActivateAnimation(string name)
    {
        OnActivateAnimation?.Invoke(name);
    }


    public void Click()
    {
        OnClick?.Invoke();
    }

    #region Output

    public event Action OnClick;

    public event Action<string> OnActivateAnimation;

    #endregion
}
