using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapOrderPresenter
{
    private readonly MapOrderModel _model;

    public MapOrderPresenter(MapOrderModel model)
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
