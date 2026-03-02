using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostessEntityPresenter : IHostessEntityControllerListener, IHostessEntityControllerProvider
{
    private readonly HostessEntityModel _model;
    private readonly HostessEntityView _view;

    public HostessEntityPresenter(HostessEntityModel model, HostessEntityView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _view.Initialize();
    }

    public void Dispsoe()
    {
        DeactivateEvents();

        _view.Dispose();
    }

    private void ActivateEvents()
    {
        _view.OnLeave += _model.LeaveVisitor;

        _model.OnHostessOpenChoose += HostessOpenChoose;
    }

    private void DeactivateEvents()
    {
        _view.OnLeave -= _model.LeaveVisitor;

        _model.OnHostessOpenChoose -= HostessOpenChoose;
    }

    #region Output

    public event Action OnHostessOpenChoose;
    public event Action OnLeave
    {
        add => _model.OnLeave += value;
        remove => _model.OnLeave -= value;
    }

    public event Action OnSuccessAssign
    {
        add => _model.OnSuccessAssign += value;
        remove => _model.OnSuccessAssign -= value;
    }

    private void HostessOpenChoose(CasinoEntityType? type)
    {
        _view.SetCasinoEntityType(type);

        OnHostessOpenChoose?.Invoke();
    }

    #endregion

    #region Input

    public void ActivateInteractiveCasinoEntity() => _model.ActivateInteractiveCasinoEntity();
    public void DeactivateInteractiveCasinoEntity() => _model.DeactivateInteractiveCasinoEntity();

    public void ActivateEntranceQueueInteractive() => _model.ActivateEntranceQueueInteractive();
    public void DeactivateEntranceQueueInteractive() => _model.DeactivateEntranceQueueInteractive();


    public void ActivateAll() => _model.ActivateAll();

    #endregion
}

public interface IHostessEntityControllerProvider
{
    public void ActivateInteractiveCasinoEntity();
    public void DeactivateInteractiveCasinoEntity();


    public void ActivateEntranceQueueInteractive();
    public void DeactivateEntranceQueueInteractive();

    void ActivateAll();
}

public interface IHostessEntityControllerListener
{
    public event Action OnHostessOpenChoose;
    public event Action OnLeave;
    public event Action OnSuccessAssign;
}
