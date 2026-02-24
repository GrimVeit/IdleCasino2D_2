using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireStaffState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;

    public HireStaffState_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
    }

    public void EnterState()
    {
        _sceneRoot.OnClickToBack_HIRE_STAFF += ChangeStateToMain;

        _sceneRoot.OpenHireStaffPanel();
        _sceneRoot.OpenBlackBackgroundPanel();
        _sceneRoot.OpenAvatarBalancePanel();
    }

    public void ExitState()
    {
        _sceneRoot.OnClickToBack_HIRE_STAFF -= ChangeStateToMain;

        _sceneRoot.CloseBlackBackgroundPanel();
        _sceneRoot.CloseHireStaffPanel();
    }

    private void ChangeStateToMain()
    {
        _machineProvider.EnterState(_machineProvider.GetState<MainState_Game>());
    }
}
