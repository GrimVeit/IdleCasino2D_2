using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressModel
{
    private readonly IMoneyProvider _moneyProvider;

    private const int maxValue = 100000000;

    public GameProgressModel(IMoneyProvider moneyProvider)
    {
        _moneyProvider = moneyProvider;

        _moneyProvider.OnChangeMoney += SetCurrentMoney;
    }

    public void Initialize()
    {
        OnSetProgress?.Invoke(_moneyProvider.Money, maxValue);
    }

    public void Dispose()
    {
        _moneyProvider.OnChangeMoney -= SetCurrentMoney;
    }

    private void SetCurrentMoney(int money)
    {
        OnSetProgress?.Invoke(money > maxValue ? maxValue : money, maxValue);
    }



    #region Output

    public event Action<int, int> OnSetProgress;

    #endregion
}
