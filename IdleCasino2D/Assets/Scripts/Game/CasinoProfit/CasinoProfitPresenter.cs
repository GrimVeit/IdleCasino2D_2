using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoProfitPresenter : ICasinoProfitListener
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

        OnChooseCasinoTypeForProfit?.Invoke();
    }

    #region Output

    public event Action OnChooseCasinoTypeForProfit;

    #endregion
}

public interface ICasinoProfitListener
{
    public event Action OnChooseCasinoTypeForProfit;
}
