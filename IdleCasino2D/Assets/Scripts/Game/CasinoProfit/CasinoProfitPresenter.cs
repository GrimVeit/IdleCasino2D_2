using System;

public class CasinoProfitPresenter : ICasinoProfitListener, ICasinoProfitProvider
{
    private readonly CasinoProfitModel _model;
    private readonly CasinoProfitView _view;

    public CasinoProfitPresenter(CasinoProfitModel model, CasinoProfitView view)
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

        _view.Dispose();
        _model.Dispose();
    }

    private void ActivateEvents()
    {
        _view.OnChooseProfitType += _model.SetCasinoType;
        _view.OnUpgrade += _model.UpgradeCurrentType;

        _model.OnChooseEntityType += SetCasinoType;
        _model.OnUpdateDetailPanel += _view.UpdateDetailPanel;
        _model.OnUpdateMain += _view.UpdatePriceMain;
    }

    private void DeactivateEvents()
    {
        _view.OnChooseProfitType -= _model.SetCasinoType;
        _view.OnUpgrade -= _model.UpgradeCurrentType;

        _model.OnChooseEntityType += SetCasinoType;
        _model.OnUpdateDetailPanel -= _view.UpdateDetailPanel;
        _model.OnUpdateMain -= _view.UpdatePriceMain;
    }


    private void SetCasinoType(CasinoEntityType entityType)
    {
        _view.SetCasinoType(entityType);

        OnChooseCasinoTypeForProfit_Type?.Invoke(entityType);
        OnChooseCasinoTypeForProfit?.Invoke();
    }

    #region Output

    public event Action<CasinoEntityType> OnChooseCasinoTypeForProfit_Type;
    public event Action OnChooseCasinoTypeForProfit;
    public event Action<CasinoEntityType, int> OnUpdate
    {
        add => _model.OnUpdateMain += value;
        remove => _model.OnUpdateMain -= value;
    }

    #endregion

    #region Input

    public void ActivateUpgrade() => _model.ActivateUpgrade();
    public void DeactivateUpgrade() => _model.DeactivateUpgrade();

    #endregion
}

public interface ICasinoProfitProvider
{
    public void ActivateUpgrade();
    public void DeactivateUpgrade();
}

public interface ICasinoProfitListener
{
    public event Action OnChooseCasinoTypeForProfit;
    public event Action<CasinoEntityType> OnChooseCasinoTypeForProfit_Type;
    public event Action<CasinoEntityType, int> OnUpdate;
}
