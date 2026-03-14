using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckProfitOnlineState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly IProfitOfflineInfo _profitOfflineInfoProvider;

    public CheckProfitOnlineState_Game(IStateMachineProvider machineProvider, IProfitOfflineInfo profitOfflineInfoProvider)
    {
        _machineProvider = machineProvider;
        _profitOfflineInfoProvider = profitOfflineInfoProvider;
    }

    public void EnterState()
    {
        if(_profitOfflineInfoProvider.IsActive && _profitOfflineInfoProvider.Earn > 30)
        {
            ChangeStateToProfit();
        }
        else
        {
            ChangeStateToMain();
        }
    }

    public void ExitState()
    {

    }

    private void ChangeStateToMain()
    {
        _machineProvider.EnterState(_machineProvider.GetState<MainState_Game>());
    }

    private void ChangeStateToProfit()
    {
        _machineProvider.EnterState(_machineProvider.GetState<ProfitOnlineState_Game>());
    }
}
