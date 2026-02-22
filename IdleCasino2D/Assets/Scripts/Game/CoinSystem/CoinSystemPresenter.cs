using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSystemPresenter : ICoinSystemProvider
{
    private readonly CoinSystemModel _model;
    private readonly CoinSystemView _view;

    public CoinSystemPresenter(CoinSystemModel model, CoinSystemView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();
    }

    public void Dispose()
    {
        DeactivateEvents();
    }

    private void ActivateEvents()
    {
        _view.OnCoinCollected += _model.AddMoney;

        _model.OnAddCoin += _view.AddCoin;
    }

    private void DeactivateEvents()
    {
        _view.OnCoinCollected -= _model.AddMoney;

        _model.OnAddCoin -= _view.AddCoin;
    }

    #region Input

    public void AddCoin(Vector3 position, int cooutValue) => _model.AddCoin(position, cooutValue);

    #endregion
}

public interface ICoinSystemProvider
{
    void AddCoin(Vector3 position, int countValue);
}
