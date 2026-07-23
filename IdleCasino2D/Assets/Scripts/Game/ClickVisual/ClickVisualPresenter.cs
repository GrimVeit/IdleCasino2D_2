using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ClickVisualPresenter : IClickVisualProvider
{
    private readonly ClickVisualView _view;

    public ClickVisualPresenter(ClickVisualView view)
    {
        _view = view;
    }

    public void Initialize()
    {
        _view.Initialize();
    }

    public void Dispose()
    {
        _view.Dispose();
    }

    #region Input

    public void Show() => _view.Show();
    public void Hide() => _view.Hide();
    public void MoveTo(Vector3 position) => _view.MoveTo(position);
    public void MoveTo(string id) => _view.MoveTo(id);
    public void SetPosition(Vector3 position) => _view.SetPosition(position);

    #endregion
}

public interface IClickVisualProvider
{
    public void Show();
    public void Hide();
    public void MoveTo(Vector3 position);
    public void MoveTo(string id);
    public void SetPosition(Vector3 position);
}
