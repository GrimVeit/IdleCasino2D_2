using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoProfitStorePresenter : ICasinoProfitStoreInfo
{
    private readonly CasinoProfitStoreModel _model;

    public CasinoProfitStorePresenter(CasinoProfitStoreModel model)
    {
        _model = model;
    }

    public void Initialize()
    {
        _model.Initialize();
    }

    public void Dispose()
    {
        _model.Dispose();
    }

    #region Input

    public int GetProfit(CasinoEntityType type) => _model.GetProfit(type);

    #endregion
}

public interface ICasinoProfitStoreInfo
{
    public int GetProfit(CasinoEntityType type);
}

public interface ICasinoProfitStoreProvider
{
    public void SetProfit(CasinoEntityType type, int profit);
}

public interface ICasinoProfitStoreListener
{
    public void OnProfitStoreChanged(CasinoEntityType entityType, int value);
}
