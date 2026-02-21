using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarPresenter : IAvatarInfoProvider, IAvatarProvider, IAvatarEventsProvider
{
    private readonly AvatarModel _model;
    private readonly AvatarView _view;

    public AvatarPresenter(AvatarModel model, AvatarView view)
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
        _model.OnSelectAvatar += _view.SetAvatar;
    }

    private void DeactivateEvents()
    {
        _model.OnSelectAvatar -= _view.SetAvatar;
    }

    #region Output

    public event Action<int> OnChooseAvatar
    {
        add => _model.OnSelectAvatar += value;
        remove => _model.OnSelectAvatar -= value;
    }

    #endregion

    #region Input

    public void SelectAvatar(int id) => _model.Select(id);
    public int GetCurrentAvatar() => _model.CurrentIndexAvatar;

    #endregion
}

public interface IAvatarInfoProvider
{
    int GetCurrentAvatar();
}

public interface IAvatarProvider
{
    public void SelectAvatar(int id);
}

public interface IAvatarEventsProvider
{
    public event Action<int> OnChooseAvatar;
}
