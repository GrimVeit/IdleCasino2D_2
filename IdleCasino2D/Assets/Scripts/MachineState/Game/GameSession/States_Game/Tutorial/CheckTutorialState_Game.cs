using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTutorialState_Game : IState
{
    private readonly IStateMachineProvider _stateMachineProvider;
    private readonly IStoreFirstLaunchProvider _storeFirstLaunchProvider;

    public CheckTutorialState_Game(IStateMachineProvider stateMachineProvider, IStoreFirstLaunchProvider storeFirstLaunchProvider)
    {
        _stateMachineProvider = stateMachineProvider;
        _storeFirstLaunchProvider = storeFirstLaunchProvider;
    }

    public void EnterState()
    {
        Debug.Log("<color=red>ACTIVATE STATE - CHECK TUTORIAL / GAME</color>");

        if (_storeFirstLaunchProvider.IsFirstLaunch)
        {
            ChangeStateToTutorial();
        }
        else
        {
            ChangeStateToStart();
        }
    }

    public void ExitState()
    {

    }

    private void ChangeStateToStart()
    {
        _stateMachineProvider.EnterState(_stateMachineProvider.GetState<StartState_Game>());
    }

    private void ChangeStateToTutorial()
    {
        _stateMachineProvider.EnterState(_stateMachineProvider.GetState<Tutorial01_Welcome_State_Game>());
    }
}
