using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapOrderPresenter
{
    private readonly MapOrderModel _model;
    private readonly MapOrderView _view;

    public MapOrderPresenter(MapOrderModel model, MapOrderView view)
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
        _view.OnRoomAdded += _model.SetRooms;
    }

    private void DeactivateEvents()
    {
        _view.OnRoomAdded -= _model.SetRooms;
    }
}
