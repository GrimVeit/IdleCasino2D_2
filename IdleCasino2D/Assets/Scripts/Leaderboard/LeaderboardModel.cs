using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardModel
{
    private readonly IDatabaseRecordsEvents _databaseRecordsEvents;
    private readonly IDatabaseProvider _databaseProvider;
    private readonly IAuthInfo _authInfo;
    private readonly ITimerListener _timerListener;
    private readonly ITimerProvider _timerProvider;

    public LeaderboardModel(IDatabaseRecordsEvents databaseRecordsEvents, IDatabaseProvider databaseProvider, IAuthInfo authInfo, ITimerListener timerListener, ITimerProvider timerProvider)
    {
        _databaseRecordsEvents = databaseRecordsEvents;
        _databaseProvider = databaseProvider;
        _authInfo = authInfo;
        _timerListener = timerListener;
        _timerProvider = timerProvider;

        _databaseRecordsEvents.OnGetUsersRecords += GetUsers;
        _timerListener.OnStopTimer += Reload;
    }

    public void Initialize()
    {
        _timerProvider.ActivateTimer(3600, TimerDirection.Backward);
    }

    public void Dispose()
    {
        _databaseRecordsEvents.OnGetUsersRecords -= GetUsers;
        _timerListener.OnStopTimer -= Reload;

    }

    private void Reload()
    {
        if(_authInfo.IsAuthorization())
           _databaseProvider.RefreshData();

        _timerProvider.ActivateTimer(3600, TimerDirection.Backward);
    }

    private void GetUsers(List<UserData> users)
    {
        var top = users.Take(10).ToList();
        OnGetTopPlayers?.Invoke(top);
    }

    #region Output

    public event Action<List<UserData>> OnGetTopPlayers;

    #endregion
}
