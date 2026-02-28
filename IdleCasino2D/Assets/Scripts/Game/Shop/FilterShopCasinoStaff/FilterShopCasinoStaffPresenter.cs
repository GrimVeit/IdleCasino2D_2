using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterShopCasinoStaffPresenter : IFilterShopCasinoStaffProvider, IFilterShopCasinoStaffListener
{
    private readonly FilterShopCasinoStaffModel _model;
    private readonly FilterShopCasinoStaffView _view;

    public FilterShopCasinoStaffPresenter(FilterShopCasinoStaffModel model, FilterShopCasinoStaffView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _model.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _model.Dispose();
    }

    private void ActivateEvents()
    {
        _model.OnPurchaseFailed += _view.SetTextFail;
        _model.OnStaffOpenChoose += StaffOpenChoose;
    }

    private void DeactivateEvents()
    {
        _model.OnPurchaseFailed -= _view.SetTextFail;
        _model.OnStaffOpenChoose -= StaffOpenChoose;
    }

    private void StaffOpenChoose(ShopCasinoStaffData data)
    {
        _view.SetData(data);

        OnStaffOpenChoose?.Invoke();
    }

    #region Output

    public event Action OnStaffOpenChoose;
    public event Action OnStaffPurchased
    {
        add => _model.OnStaffPurchased += value;
        remove => _model.OnStaffPurchased -= value;
    }

    #endregion

    #region Input

    public void Activate() => _model.Activate();
    public void Deactivate() => _model.Deactivate();

    public void CancelSelection() => _model.CancelSelection();
    public void ClearFailText() => _view.ClearFailText();

    #endregion
}

public interface IFilterShopCasinoStaffProvider
{
    public void CancelSelection();
    public void ClearFailText();

    public void Activate();
    public void Deactivate();
}

public interface IFilterShopCasinoStaffListener
{
    public event Action OnStaffOpenChoose;
    public event Action OnStaffPurchased;
}
