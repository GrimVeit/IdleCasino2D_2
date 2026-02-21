using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerPresenter : IDealer
{
    private readonly DealerModel _model;
    private readonly DealerView _view;

    public DealerPresenter(DealerModel model, DealerView view)
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
        _model.OnSetPlay += _view.Play;
        _model.OnSetIdle += _view.Idle;
    }

    private void DeactivateEvents()
    {
        _model.OnSetPlay -= _view.Play;
        _model.OnSetIdle -= _view.Idle;
    }

    #region DEALER

    public void SetIdle() => _model.SetIdle();

    public void SetPlay() => _model.SetPlay();

    #endregion
}
