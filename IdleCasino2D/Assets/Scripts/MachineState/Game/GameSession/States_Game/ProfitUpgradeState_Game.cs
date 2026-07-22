using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfitUpgradeState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly ICasinoProfitProvider _casinoProfitProvider;

    public ProfitUpgradeState_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot, ICasinoProfitProvider casinoProfitProvider)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
        _casinoProfitProvider = casinoProfitProvider;
    }

    public void EnterState()
    {
        _sceneRoot.OnClickToBack_UPGRADE_PROFIT += ChangeStateToUpgrade;

        _casinoProfitProvider.ActivateUpgrade();
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
