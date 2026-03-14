using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankTransactionVisualModel
{
    private readonly IMoneyProvider _moneyProvider;

    public BankTransactionVisualModel(IMoneyProvider moneyProvider)
    {
        _moneyProvider = moneyProvider;

        _moneyProvider.OnSendMoney += SetTransaction;
    }

    public void Initialize()
    {

    }

    public void Dispose()
    {
        _moneyProvider.OnSendMoney -= SetTransaction;
    }

    #region Output

    public event Action<int> OnSetTransaction;

    private void SetTransaction(int value)
    {
        OnSetTransaction?.Invoke(value);
    }

    #endregion
}
