using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorPathTrafficPresenter
{
    private readonly VisitorPathTrafficModel _model;

    public VisitorPathTrafficPresenter(VisitorPathTrafficModel model)
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
}
