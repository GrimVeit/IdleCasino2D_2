using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfitOnlineState_Game : IState
{
    private readonly IStateMachineProvider _stateMachineProvider;
    private readonly IProfitOfflineListener _profitOfflineListener;
    private readonly UIGameRoot _sceneRoot;

    public ProfitOnlineState_Game(IStateMachineProvider stateMachineProvider, IProfitOfflineListener profitOfflineListener, UIGameRoot sceneRoot)
    {
        _stateMachineProvider = stateMachineProvider;
        _profitOfflineListener = profitOfflineListener;
        _sceneRoot = sceneRoot;
    }

    public void EnterState()
    {
        _profitOfflineListener.OnCollectProfit += ChangeStateToMain;

        _sceneRoot.OpenOfflineEarningsPanel();
        _sceneRoot.OpenBlackBackgroundPanel();
        _sceneRoot.OpenAvatarBalancePanel();
    }

    public void ExitState()
    {
        _profitOfflineListener.OnCollectProfit -= ChangeStateToMain;

        _sceneRoot.CloseOfflineEarningsPanel();
        _sceneRoot.CloseBlackBackgroundPanel();
    }

    private void ChangeStateToMain()
    {
        _stateMachineProvider.EnterState(_stateMachineProvider.GetState<MainState_Game>());
    }
}
