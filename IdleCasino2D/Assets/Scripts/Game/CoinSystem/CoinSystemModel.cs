using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSystemModel
{
    private readonly IMoneyProvider _moneyProvider;

    public CoinSystemModel(IMoneyProvider moneyProvider)
    {
        _moneyProvider = moneyProvider;
    }

    public void Initialize() { }

    public void Dispose() { }

    public void AddMoney(int amount)
    {
        Debug.Log("ADD MONEY " + amount);

        _moneyProvider.SendMoney(amount);
    }

    public void AddCoin(Vector3 position, int countValue)
    {
        OnAddCoin?.Invoke(position, countValue);
    }

    #region Output 

    public event Action<Vector3, int> OnAddCoin;

    #endregion
}
