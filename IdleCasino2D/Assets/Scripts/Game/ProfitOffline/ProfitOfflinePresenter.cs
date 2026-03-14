using System;

public class ProfitOfflinePresenter : IProfitOfflineInfo, IProfitOfflineListener
{
    private readonly ProfitOfflineModel _model;
    private readonly ProfitOfflineView _view;

    public ProfitOfflinePresenter(ProfitOfflineModel model, ProfitOfflineView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _model.Initialize();
        _view.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _model.Dispose();
        _view.Dispose();
    }

    private void ActivateEvents()
    {
        _view.OnClickToCollect += _model.CollectProfit;

        _model.OnOfflineProfitCalculated += _view.SetEarn;
    }

    private void DeactivateEvents()
    {
        _view.OnClickToCollect -= _model.CollectProfit;

        _model.OnOfflineProfitCalculated -= _view.SetEarn;
    }

    public void Save() => _model.Dispose();

    #region INFO

    public bool IsActive => _model.IsActive;
    public int Earn => _model.Earn;

    #endregion

    #region LISTENER

    public event Action OnCollectProfit
    {
        add => _model.OnCollectProfit += value;
        remove => _model.OnCollectProfit -= value;
    }

    #endregion
}

public interface IProfitOfflineInfo
{
    public bool IsActive { get; }
    public int Earn { get; }
}

public interface IProfitOfflineListener
{
    public event Action OnCollectProfit;
}
