using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarVisualPresenter
{
    private readonly AvatarVisualModel _model;
    private readonly AvatarVisualView _view;

    public AvatarVisualPresenter(AvatarVisualModel model, AvatarVisualView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _view.Initialize();
        _model.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _view.Dispose();
        _model.Dispose();
    }

    private void ActivateEvents()
    {
        _view.OnChooseAvatar += _model.SelectAvatar;

        _model.OnSelectAvatar += _view.Select;
        _model.OnDeselectAvatar += _view.Deselect;
    }

    private void DeactivateEvents()
    {
        _view.OnChooseAvatar -= _model.SelectAvatar;

        _model.OnSelectAvatar -= _view.Select;
        _model.OnDeselectAvatar -= _view.Deselect;
    }
}
