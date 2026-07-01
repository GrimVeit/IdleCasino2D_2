using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardView : View
{
    [SerializeField] private Transform transformContent;
    [SerializeField] private UserGrid userGridPrefab_3Laurel;
    [SerializeField] private UserGrid userGridPrefab_2Laurel;
    [SerializeField] private UserGrid userGridPrefab_1Laurel;

    public void GetTopPlayers(List<UserData> users)
    {
        for (int i = 0; i < users.Count; i++)
        {
            UserGrid grid;

            if (i == 0)
            {
                grid = Instantiate(userGridPrefab_3Laurel, transformContent);
                grid.SetData(users[i].Nickname, users[i].Record);
            }
            else if(i == 1)
            {
                grid = Instantiate(userGridPrefab_2Laurel, transformContent);
                grid.SetData(users[i].Nickname, users[i].Record);
            }
            else
            {
                grid = Instantiate(userGridPrefab_1Laurel, transformContent);
                grid.SetData(users[i].Nickname, users[i].Record);
            }
        }
    }
}
