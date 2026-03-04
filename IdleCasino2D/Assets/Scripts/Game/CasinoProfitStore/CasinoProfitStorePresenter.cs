using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CasinoProfitStorePresenter : ICasinoProfitStoreInfo, ICasinoProfitStoreProvider, ICasinoProfitStoreListener
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

    #region Output

    public event Action<CasinoEntityType, int> OnProfitStoreChanged
    {
        add => _model.OnChangeProfitValue += value;
        remove => _model.OnChangeProfitValue -= value;
    }

    #endregion

    #region Input

    public int GetProfit(CasinoEntityType type) => _model.GetProfit(type);
    public void SetProfit(CasinoEntityType type, int profit) => _model.SetProfit(type, profit);

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
    public event Action<CasinoEntityType, int> OnProfitStoreChanged;
}
