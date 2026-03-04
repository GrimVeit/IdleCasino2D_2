using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly ICasinoProfitListener _casinoProfitListener;

    public UpgradeState_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot, ICasinoProfitListener casinoProfitListener)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
        _casinoProfitListener = casinoProfitListener;
    }

    public void EnterState()
    {
        _casinoProfitListener.OnChooseCasinoTypeForProfit += ChangeStateToUpgradeProfit;
        _sceneRoot.OnClickToBack_UPGRADE += ChangeStateToMain;

        _sceneRoot.OpenUpgradePanel();
        _sceneRoot.OpenBlackBackgroundPanel();
        _sceneRoot.OpenAvatarBalancePanel();
    }

    public void ExitState()
    {
        _casinoProfitListener.OnChooseCasinoTypeForProfit -= ChangeStateToUpgradeProfit;
        _sceneRoot.OnClickToBack_UPGRADE -= ChangeStateToMain;

        _sceneRoot.CloseUpgradePanel();
    }

    private void ChangeStateToMain()
    {
        _sceneRoot.CloseBlackBackgroundPanel();

        _machineProvider.EnterState(_machineProvider.GetState<MainState_Game>());
    }

    private void ChangeStateToUpgradeProfit()
    {
        _machineProvider.EnterState(_machineProvider.GetState<ProfitUpgradeState_Game>());
    }
}
