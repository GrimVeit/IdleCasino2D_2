using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarBalancePanel_Game : MovePanel
{
    [SerializeField] private Button buttonLeaderboard;

    public override void Initialize()
    {
        base.Initialize();

        buttonLeaderboard.onClick.AddListener(ClickLeaderboard);
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonLeaderboard.onClick.RemoveListener(ClickLeaderboard);
    }

    #region Output

    public event Action OnClickToLeaderboard;

    private void ClickLeaderboard()
    {
        Debug.Log("ClickLeader");

        OnClickToLeaderboard?.Invoke();
    }

    #endregion
}
