using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerVisitorPresenter : ISpawnerVisitorProvider, ISpawnerVisitorListener, ISpawnerVisitorInfoProvider
{
    private readonly SpawnerVisitorModel _model;
    private readonly SpawnerVisitorView _view;

    public SpawnerVisitorPresenter(SpawnerVisitorModel model, SpawnerVisitorView view)
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
        _view.OnSpawnVisitor += _model.AddVisitor;
        _view.OnDestroyVisitor += _model.RemoveVisitor;

        _model.OnSpawnVisitor += _view.SpawnVisitor;
        _model.OnDestroyVisitor += _view.DestroyVisitor;
    }

    private void DeactivateEvents()
    {
        _view.OnSpawnVisitor -= _model.AddVisitor;
        _view.OnDestroyVisitor -= _model.RemoveVisitor;

        _model.OnSpawnVisitor -= _view.SpawnVisitor;
        _model.OnDestroyVisitor -= _view.DestroyVisitor;
    }


    #region Output

    public event Action<IVisitor> OnAddVisitor
    {
        add => _model.OnAddVisitor += value;
        remove => _model.OnAddVisitor -= value;
    }

    public event Action<IVisitor> OnRemoveVisitor
    {
        add => _model.OnDestroyVisitor += value;
        remove => _model.OnDestroyVisitor -= value;
    }

    public int CountVisitors => _model.CountVisitors;

    #endregion


    #region Input

    public void SpawnVisitor() => _model.SpawnVisitor();
    public void DestroyVisitor(IVisitor visitor) => _model.DestroyVisitor(visitor);

    #endregion
}

public interface ISpawnerVisitorInfoProvider
{
    int CountVisitors { get; }
}

public interface ISpawnerVisitorProvider
{
    void SpawnVisitor();
    void DestroyVisitor(IVisitor visitor);
}

public interface ISpawnerVisitorListener
{
    event Action<IVisitor> OnAddVisitor;
    event Action<IVisitor> OnRemoveVisitor;
}
