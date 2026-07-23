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
    private readonly IClickVisualProvider _clickVisualProvider;

    public Tutorial04_HireStaff_ChooseStaffType_State_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot, IShopCasinoPersonalListener shopCasinoPersonalListener, IAdministratorVisualProvider administratorVisualProvider, IClickVisualProvider clickVisualProvider)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
        _shopCasinoPersonalListener = shopCasinoPersonalListener;
        _administratorVisualProvider = administratorVisualProvider;
        _clickVisualProvider = clickVisualProvider;
    }

    public void EnterState()
    {
        _shopCasinoPersonalListener.OnChooseShopPersonalGroup += ChangeStateToSelectStaff;

        _administratorVisualProvider.Activate();
        _sceneRoot.OpenHireStaffPanel();
        _sceneRoot.OpenBlackBackgroundPanel();
        _clickVisualProvider.MoveTo("MANAGER_STAFFPANEL");
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
