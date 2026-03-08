using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSpotPresenter : IGameSpot
{
    private readonly SlotSpotModel _model;
    private readonly SlotSpotView _view;

    public SlotSpotPresenter(SlotSpotModel model, SlotSpotView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _view.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _view.Dispose();
    }

    private void ActivateEvents()
    {
        _view.OnClick += _model.Click;

        _model.OnActivateAnimation += _view.SetAnimation;
    }

    private void DeactivateEvents()
    {
        _view.OnClick -= _model.Click;

        _model.OnActivateAnimation -= _view.SetAnimation;
    }

    #region Output

    public event Action OnClick
    {
        add => _model.OnClick += value;
        remove => _model.OnClick -= value;
    }

    #endregion

    #region Input
    public void ActivateAnimation(string name) => _model.ActivateAnimation(name);

    public void ActivateHightlight() => _view.ActivateHighlight();
    public void DeactivateHighlight() => _view.DeactivateHighlight();

    #endregion
}
