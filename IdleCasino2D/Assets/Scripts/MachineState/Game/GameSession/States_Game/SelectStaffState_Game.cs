using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStaffState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;

    public SelectStaffState_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
    }

    public void EnterState()
    {
        _sceneRoot.OnClickToBack_SELECT_STAFF += ChangeStateToHireStaff;

        _sceneRoot.OpenSelectStaffPanel();
    }

    public void ExitState()
    {
        _sceneRoot.OnClickToBack_SELECT_STAFF -= ChangeStateToHireStaff;

        _sceneRoot.CloseSelectStaffPanel();
    }

    private void ChangeStateToHireStaff()
    {
        _machineProvider.EnterState(_machineProvider.GetState<HireStaffState_Game>());
    }
}
