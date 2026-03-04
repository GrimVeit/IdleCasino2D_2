using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoProfitModel
{
    private readonly ICasinoProfitStoreProvider _casinoProfitStoreProvider;

    public CasinoProfitModel(ICasinoProfitStoreProvider casinoProfitStoreProvider)
    {
        _casinoProfitStoreProvider = casinoProfitStoreProvider;
    }

    public void SetCasinoType(CasinoEntityType casinoEntityType)
    {
        OnChooseEntityType?.Invoke(casinoEntityType);
    }

    #region Output

    public event Action<CasinoEntityType> OnChooseEntityType;

    #endregion
}
