using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireStaffState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly IShopCasinoPersonalListener _shopCasinoPersonalListener;

    public HireStaffState_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot, IShopCasinoPersonalListener shopCasinoPersonalListener)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
        _shopCasinoPersonalListener = shopCasinoPersonalListener;
    }

    public void EnterState()
    {
        _shopCasinoPersonalListener.OnChoosePersonalGroup += ChangeStateToSelectStaff;
        _sceneRoot.OnClickToBack_HIRE_STAFF += ChangeStateToMain;

        _sceneRoot.OpenHireStaffPanel();
        _sceneRoot.OpenBlackBackgroundPanel();
        _sceneRoot.OpenAvatarBalancePanel();
    }

    public void ExitState()
    {
        _shopCasinoPersonalListener.OnChoosePersonalGroup -= ChangeStateToSelectStaff;
        _sceneRoot.OnClickToBack_HIRE_STAFF -= ChangeStateToMain;

        _sceneRoot.CloseHireStaffPanel();
    }

    private void ChangeStateToSelectStaff()
    {
        _machineProvider.EnterState(_machineProvider.GetState<SelectStaffState_Game>());
    }

    private void ChangeStateToMain()
    {
        _sceneRoot.CloseBlackBackgroundPanel();

        _machineProvider.EnterState(_machineProvider.GetState<MainState_Game>());
    }
}
