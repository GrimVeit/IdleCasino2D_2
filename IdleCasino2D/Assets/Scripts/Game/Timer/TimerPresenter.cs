using System;

public class TimerPresenter : ITimerProvider, ITimerListener
{
    private readonly TimerModel _model;
    private readonly ITimerView _view;

    public TimerPresenter(TimerModel model, ITimerView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _view.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _view.Dispose();
    }

    private void ActivateEvents()
    {
        _model.OnActivateTimer += _view.ActivateTimer;
        _model.OnDeactivateTimer += _view.DeactivateTimer;

        _model.OnTimeChanged += _view.ChangeTime;
    }

    private void DeactivateEvents()
    {
        _model.OnActivateTimer -= _view.ActivateTimer;
        _model.OnDeactivateTimer -= _view.DeactivateTimer;

        _model.OnTimeChanged -= _view.ChangeTime;
    }

    public void ActivateTimer(int seconds, TimerDirection direction)
    {
        _model.ActivateTimer(seconds, direction);
    }

    public void DeactivateTimer()
    {
        _model.DeactivateTimer();
    }

    public void ResetTimer()
    {
        _model.ResetTimer();
    }

    #region Output


    public event Action<int> OnTimeChanged
    {
        add => _model.OnTimeChanged += value;
        remove => _model.OnTimeChanged -= value;
    }


    public event Action OnStopTimer
    {
        add { _model.OnStopTimer += value; }
        remove { _model.OnStopTimer -= value; }
    }

    #endregion
}

public interface ITimerView
{
    void Initialize();
    void Dispose();

    void ChangeTime(int sec);
    void ActivateTimer();
    void DeactivateTimer();
}

public interface ITimerProvider
{
    void ActivateTimer(int seconds, TimerDirection direction);
    void DeactivateTimer();
    void ResetTimer();
}

public interface ITimerListener
{
    public event Action OnStopTimer;
    public event Action<int> OnTimeChanged;
}
