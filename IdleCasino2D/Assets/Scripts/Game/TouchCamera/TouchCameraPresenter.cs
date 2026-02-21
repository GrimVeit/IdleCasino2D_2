using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCameraPresenter : ITouchCameraProvider
{
    private readonly TouchCameraView _view;

    public TouchCameraPresenter(TouchCameraView view)
    {
        _view = view;
    }

    public void Initialize()
    {
        _view.Initialize();
    }

    public void Dispose()
    {

    }

    #region Input

    public void ActivateInteractive() => _view.ActivateInteractive();
    public void DeactivateInteractive() => _view.DeactivateInteractive();

    public void SetPosition(Vector3 position) => _view.SetPosition(position);

    #endregion
}

public interface ITouchCameraProvider
{
    public void ActivateInteractive();
    public void DeactivateInteractive();

    public void SetPosition(Vector3 position);
}
