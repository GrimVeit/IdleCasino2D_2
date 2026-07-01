using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardPresenter
{
    private readonly LeaderboardModel _model;
    private readonly LeaderboardView _view;

    public LeaderboardPresenter(LeaderboardModel model, LeaderboardView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _model.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _model.Dispose();
    }

    private void ActivateEvents()
    {
        _model.OnGetTopPlayers += _view.GetTopPlayers;
    }

    private void DeactivateEvents()
    {
        _model.OnGetTopPlayers -= _view.GetTopPlayers;
    }
}
