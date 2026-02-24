using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;

    public UpgradeState_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
    }

    public void EnterState()
    {
        _sceneRoot.OnClickToBack_UPGRADE += ChangeStateToMain;

        _sceneRoot.OpenUpgradePanel();
        _sceneRoot.OpenBlackBackgroundPanel();
        _sceneRoot.OpenAvatarBalancePanel();
    }

    public void ExitState()
    {
        _sceneRoot.OnClickToBack_UPGRADE -= ChangeStateToMain;

        _sceneRoot.CloseBlackBackgroundPanel();
        _sceneRoot.CloseUpgradePanel();
    }

    private void ChangeStateToMain()
    {
        _machineProvider.EnterState(_machineProvider.GetState<MainState_Game>());
    }
}
