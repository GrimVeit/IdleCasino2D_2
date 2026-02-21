using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPresenter : IVideoProvider
{
    private readonly VideoModel _model;
    private readonly VideoView _view;

    public VideoPresenter(VideoModel model, VideoView view)
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
        _model.OnPlay += _view.Play;
        _model.OnPrepare += _view.Prepare;
    }

    private void DeactivateEvents()
    {
        _model.OnPlay -= _view.Play;
        _model.OnPrepare -= _view.Prepare;
    }

    #region Input

    public void Play(string id, Action onComplete) => _model.Play(id, onComplete);
    public void Prepare(string id) => _model.Prepare(id);

    #endregion
}

public interface IVideoProvider
{
    public void Prepare(string id);
    public void Play(string id, Action onComplete = null);
}
