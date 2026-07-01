using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;

    public LeaderboardState_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot) 
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
    }

    public void EnterState()
    {
        _sceneRoot.OnClickToBack_LEADER += ChangeStateToMain;

        _sceneRoot.OpenLeaderboardPanel();
        _sceneRoot.OpenBlackBackgroundPanel();
        _sceneRoot.OpenAvatarBalancePanel();
    }

    public void ExitState()
    {
        _sceneRoot.OnClickToBack_LEADER -= ChangeStateToMain;

        _sceneRoot.CloseBlackBackgroundPanel();
        _sceneRoot.CloseLeaderboardPanel();
    }

    private void ChangeStateToMain()
    {
        _machineProvider.EnterState(_machineProvider.GetState<MainState_Game>());
    }
}
