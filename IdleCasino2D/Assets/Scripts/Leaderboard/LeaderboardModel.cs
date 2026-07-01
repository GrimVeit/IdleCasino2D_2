using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardModel
{
    private readonly IDatabaseRecordsEvents _databaseRecordsEvents;

    private bool isActive = true;

    public LeaderboardModel(IDatabaseRecordsEvents databaseRecordsEvents)
    {
        _databaseRecordsEvents = databaseRecordsEvents;
        _databaseRecordsEvents.OnGetUsersRecords += GetUsers;
    }

    public void Initialize()
    {

    }

    public void Dispose()
    {
        _databaseRecordsEvents.OnGetUsersRecords -= GetUsers;
    }

    private void GetUsers(List<UserData> users)
    {
        var top = users.Take(5).ToList();
        OnGetTopPlayers?.Invoke(top);
    }

    public void Activate()
    {
        isActive = true;
    }

    #region Output

    public event Action<List<UserData>> OnGetTopPlayers;

    #endregion
}
