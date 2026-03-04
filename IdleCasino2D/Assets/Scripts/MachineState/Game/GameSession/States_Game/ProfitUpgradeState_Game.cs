using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfitUpgradeState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;

    public ProfitUpgradeState_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
    }

    public void EnterState()
    {
        _sceneRoot.OnClickToBack_UPGRADE_PROFIT += ChangeStateToUpgrade;

        _sceneRoot.OpenProfitUpgradePanel();
    }

    public void ExitState()
    {
        _sceneRoot.OnClickToBack_UPGRADE_PROFIT -= ChangeStateToUpgrade;

        _sceneRoot.CloseProfitUpgradePanel();
    }

    private void ChangeStateToUpgrade()
    {
        _machineProvider.EnterState(_machineProvider.GetState<UpgradeState_Game>());
    }
}
