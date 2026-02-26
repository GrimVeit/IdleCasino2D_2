using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCasinoSpotPresenter : IShopCasinoEntitySpotListener, IShopCasinoEntitySpotProvider
{
    private readonly ShopCasinoSpotModel _model;
    private readonly ShopCasinoSpotView _view;

    public ShopCasinoSpotPresenter(ShopCasinoSpotModel model, ShopCasinoSpotView view)
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
        _view.OnClickToBuySpot += _model.Buy;

        _model.OnSetCasinoEntityData += SetCasinoData;
    }

    private void DeactivateEvents()
    {
        _view.OnClickToBuySpot -= _model.Buy;

        _model.OnSetCasinoEntityData -= SetCasinoData;
    }

    private void SetCasinoData(ShopCasinoEntityDataSO dataSO)
    {
        _view.SetSpotData(dataSO);

        OnSetData?.Invoke();
    }

    #region Output

    public event Action OnSetData;
    public event Action OnBuy
    {
        add => _model.OnBuy += value;
        remove => _model.OnBuy -= value;
    }

    #endregion

    #region Input

    public void ActivateListener() => _model.ActivateListener();
    public void DeactivateListener() => _model.DeactivateListener();

    #endregion
}

public interface IShopCasinoEntitySpotListener
{
    public event Action OnSetData;
    public event Action OnBuy;
}

public interface IShopCasinoEntitySpotProvider
{
    public void ActivateListener();
    public void DeactivateListener();
}
