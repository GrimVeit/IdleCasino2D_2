using System;

public class InternetPresenter
{
    private readonly InternetModel _model;
    private readonly InternetView _view;

    public InternetPresenter(InternetModel internetModel, InternetView internetView)
    {
        _model = internetModel;
        _view = internetView;
    }

    public void Initialize()
    {
        ActivateEvents();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _model.Dispose();
    }

    private void ActivateEvents()
    {
        _model.OnGetStatusDescription += _view.OnGetStatusDescription;
    }

    private void DeactivateEvents()
    {
        _model.OnGetStatusDescription -= _view.OnGetStatusDescription;
    }

    public void StartCheckConnection()
    {
        _model.StartCheckConnection();
    }

    public event Action OnInternetUnavailable
    {
        add { _model.OnInternetUnvailable += value; }
        remove { _model.OnInternetUnvailable -= value; }
    }

    public event Action OnInternetAvailable
    {
        add { _model.OnInternetAvailable += value; }
        remove { _model.OnInternetAvailable += value; }
    }
}
