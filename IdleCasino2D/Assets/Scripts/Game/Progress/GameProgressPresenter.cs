using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressPresenter
{
    private readonly GameProgressModel _model;
    private readonly GameProgressView _view;

    public GameProgressPresenter(GameProgressModel model, GameProgressView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _model.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _model.Dispose();
    }

    private void ActivateEvents()
    {
        _model.OnSetProgress += _view.SetProgress;
    }

    private void DeactivateEvents()
    {
        _model.OnSetProgress -= _view.SetProgress;
    }
}
