using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdministratorPresenter : IAdministratorVisualProvider
{
    private readonly AdministratorView _view;

    public AdministratorPresenter(AdministratorView view)
    {
        _view = view;
    }

    public void Initialize()
    {

    }

    public void Dispose()
    {

    }

    #region Input

    public void Activate() => _view.Activate();

    public void Deactivate() => _view.Deactivate();

    #endregion
}

public interface IAdministratorVisualProvider
{
    void Activate();
    void Deactivate();
}
