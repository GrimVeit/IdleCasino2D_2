using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardView : View
{
    [SerializeField] private List<LeaderboardUser> leaderboardUsers = new();
    [SerializeField] private GameObject gameObjectHeader;
    [SerializeField] private UIEffect effectNoneInternet;
    [SerializeField] private UIEffectCombination combination;

    public void Initialize()
    {
        combination.Initialize();
        effectNoneInternet.Initialize();

        effectNoneInternet.ActivateEffect();
    }

    public void Dispose()
    {
        combination.Dispose();
    }

    public void GetTopPlayers(List<UserData> users)
    {
        combination.ResetEffects();
        effectNoneInternet.DeactivateEffect();

        for (int i = 0; i < leaderboardUsers.Count; i++)
        {
            leaderboardUsers[i].Clear();
        }

        for (int i = 0; i < users.Count; i++)
        {
            leaderboardUsers[i].SetData(users[i].Nickname, users[i].Record);
        }

        if(users.Count > 3)
        {
            gameObjectHeader.SetActive(true);
        }
        else
        {
            gameObjectHeader.SetActive(false);
        }

        combination.ActivateEffect();
    }
}
