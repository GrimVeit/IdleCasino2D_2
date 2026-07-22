using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreFirstLaunchPresenter : IStoreFirstLaunchProvider
{
    private readonly StoreFirstLaunchModel _model;

    public StoreFirstLaunchPresenter(StoreFirstLaunchModel model)
    {
        _model = model;
    }

    public void Initialize()
    {

    }

    public void Dispose()
    {

    }

    #region Input

    public bool IsFirstLaunch => _model.IsFirstLaunch;
    public void CompleteFirstLaunch() => _model.CompleteFirstLaunch();

    #endregion
}

public interface IStoreFirstLaunchProvider
{
    public bool IsFirstLaunch { get; }
    public void CompleteFirstLaunch();
}
