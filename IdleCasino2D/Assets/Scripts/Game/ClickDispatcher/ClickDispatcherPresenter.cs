using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDispatcherPresenter : IClickDispatcherProvider
{
    private readonly ClickDispatcherModel _model;

    public ClickDispatcherPresenter(ClickDispatcherModel model)
    {
        _model = model;
    }

    public void Activate() => _model.Activate();
    public void Deactivate() => _model.Deactivate();
}

public interface IClickDispatcherProvider
{
    void Activate();
    void Deactivate();
}
