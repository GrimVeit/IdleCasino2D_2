using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial03_Upgrade_ChooseUpgradeType_State_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly ICasinoProfitListener _casinoProfitListener;
    private readonly IAdministratorVisualProvider _administratorVisualProvider;

    public Tutorial03_Upgrade_ChooseUpgradeType_State_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot, ICasinoProfitListener casinoProfitListener, IAdministratorVisualProvider administratorVisualProvider)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
        _casinoProfitListener = casinoProfitListener;
        _administratorVisualProvider = administratorVisualProvider;
    }

    public void EnterState()
    {
        _casinoProfitListener.OnChooseCasinoTypeForProfit_Type += ChangeStateToUpgradeProfit;

        _administratorVisualProvider.Activate();
        _sceneRoot.OpenUpgradePanel();
        _sceneRoot.OpenBlackBackgroundPanel();
    }

    public void ExitState()
    {
        _casinoProfitListener.OnChooseCasinoTypeForProfit_Type -= ChangeStateToUpgradeProfit;

        _sceneRoot.CloseUpgradePanel();
    }

    private void ChangeStateToUpgradeProfit(CasinoEntityType type)
    {
        if(type == CasinoEntityType.Slot)
        {
            ChangeStateToTutorial3_Upgrade();
        }
    }

    private void ChangeStateToTutorial3_Upgrade()
    {
        _machineProvider.EnterState(_machineProvider.GetState<Tutorial03_Upgrade_Finish_State_Game>());
    }
}
