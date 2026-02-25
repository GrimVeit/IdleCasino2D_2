using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoEntityFinancePresenter
{
    private readonly CasinoEntityFinanceModel _model;

    public CasinoEntityFinancePresenter(CasinoEntityFinanceModel model)
    {
        _model = model;
    }

    public void Initialize()
    {

    }

    public void Dispose()
    {
        _model.Dispose();
    }
}
