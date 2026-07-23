using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial04_HireStaff_ChooseStaffType_State_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly IShopCasinoPersonalListener _shopCasinoPersonalListener;
    private readonly IAdministratorVisualProvider _administratorVisualProvider;

    public Tutorial04_HireStaff_ChooseStaffType_State_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot, IShopCasinoPersonalListener shopCasinoPersonalListener, IAdministratorVisualProvider administratorVisualProvider)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
        _shopCasinoPersonalListener = shopCasinoPersonalListener;
        _administratorVisualProvider = administratorVisualProvider;
    }

    public void EnterState()
    {
        _shopCasinoPersonalListener.OnChooseShopPersonalGroup += ChangeStateToSelectStaff;

        _administratorVisualProvider.Activate();
        _sceneRoot.OpenHireStaffPanel();
        _sceneRoot.OpenBlackBackgroundPanel();
    }

    public void ExitState()
    {
        _shopCasinoPersonalListener.OnChooseShopPersonalGroup -= ChangeStateToSelectStaff;

        _sceneRoot.CloseHireStaffPanel();
    }

    private void ChangeStateToSelectStaff(ShopCasinoPersonalDataGroup group)
    {
        if(group.PersonalType == StaffType.Manager)
        {
            ChnageStateToTutorial4_Finish();
        }
    }

    private void ChnageStateToTutorial4_Finish()
    {
        _machineProvider.EnterState(_machineProvider.GetState<Tutorial04_HireStaff_Finish_State_Game>());
    }
}
