using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial05_Leaderboard_Finish_State_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly IAdministratorVisualProvider _administratorVisualProvider;

    public Tutorial05_Leaderboard_Finish_State_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot, IAdministratorVisualProvider administratorVisualProvider)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
        _administratorVisualProvider = administratorVisualProvider;
    }

    public void EnterState()
    {
        Debug.Log("<color=red>ACTIVATE STATE - TUTORIAL 05 FIRST TABLES / GAME</color>");

        _administratorVisualProvider.Activate();
        _sceneRoot.OpenLeaderboardPanel();
        _sceneRoot.OpenBlackBackgroundPanel();
    }

    public void ExitState()
    {

    }

    private void ChangeStateToMain()
    {
        _machineProvider.EnterState(_machineProvider.GetState<MainState_Game>());
    }
}
