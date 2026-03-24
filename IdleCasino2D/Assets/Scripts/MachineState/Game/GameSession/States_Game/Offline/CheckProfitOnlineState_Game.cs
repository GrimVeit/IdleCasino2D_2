using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckProfitOnlineState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly IProfitOfflineInfo _profitOfflineInfoProvider;
    private readonly ISoundProvider _soundProvider;
    private readonly ISound _soundBackground;
    private readonly ISound _soundStart;

    public CheckProfitOnlineState_Game(IStateMachineProvider machineProvider, IProfitOfflineInfo profitOfflineInfoProvider, ISoundProvider soundProvider)
    {
        _machineProvider = machineProvider;
        _profitOfflineInfoProvider = profitOfflineInfoProvider;
        _soundProvider = soundProvider;

        _soundBackground = _soundProvider.GetSound("Background");
        _soundStart = _soundProvider.GetSound("ClickStart");
    }

    public void EnterState()
    {
        if(_profitOfflineInfoProvider.IsActive && _profitOfflineInfoProvider.Earn > 30)
        {
            ChangeStateToProfit();
        }
        else
        {
            _soundBackground.SetVolume(0.25f, 0.1f, 0.01f, () =>
            {
                _soundStart.Play();
                _soundBackground.SetVolume(0.1f, 0.25f, 0.2f, 1f);
            });

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
