using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoEntityStorePresenter
{
    private readonly CasinoEntityStoreModel _model;

    public CasinoEntityStorePresenter(CasinoEntityStoreModel model)
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
}
