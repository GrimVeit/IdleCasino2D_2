using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoEntityClickInteractionPresenter : IListenerClickCasinoEntitySpot
{
    private readonly CasinoEntityClickInteractionModel _model;

    public CasinoEntityClickInteractionPresenter(CasinoEntityClickInteractionModel model)
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

    public event Action<CasinoEntityClickInteractionAdapter> OnClickToOpenCasinoEntity
    {
        add => _model.OnClickToOpenCasinoEntity += value;
        remove => _model.OnClickToOpenCasinoEntity -= value;
    }

    public event Action<CasinoEntityClickInteractionAdapter> OnClickToCloseCasinoEntity
    {
        add => _model.OnClickToCloseCasinoEntity += value;
        remove => _model.OnClickToCloseCasinoEntity -= value;
    }

    #endregion
}

public interface IListenerClickCasinoEntitySpot
{
    public event Action<CasinoEntityClickInteractionAdapter> OnClickToOpenCasinoEntity;
    public event Action<CasinoEntityClickInteractionAdapter> OnClickToCloseCasinoEntity;
}
