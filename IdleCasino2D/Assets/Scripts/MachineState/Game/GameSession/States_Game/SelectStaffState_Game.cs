using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStaffState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly IFilterShopCasinoStaffProvider _filterShopCasinoStaffActivatorProvider;
    private readonly IFilterShopCasinoStaffListener _filterShopCasinoStaffListener;

    public SelectStaffState_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot, IFilterShopCasinoStaffProvider filterShopCasinoStaffActivatorProvider, IFilterShopCasinoStaffListener filterShopCasinoStaffListener)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
        _filterShopCasinoStaffActivatorProvider = filterShopCasinoStaffActivatorProvider;
        _filterShopCasinoStaffListener = filterShopCasinoStaffListener;
    }

    public void EnterState()
    {
        _filterShopCasinoStaffListener.OnStaffOpenChoose += ChangeStateToChooseSpotStaff;
        _filterShopCasinoStaffListener.OnStaffPurchased += ChangeStateToMainState;
        _sceneRoot.OnClickToBack_SELECT_STAFF += ChangeStateToHireStaff;

        _sceneRoot.OpenSelectStaffPanel();
    }

    public void ExitState()
    {
        _filterShopCasinoStaffListener.OnStaffOpenChoose -= ChangeStateToChooseSpotStaff;
        _filterShopCasinoStaffListener.OnStaffPurchased -= ChangeStateToMainState;
        _sceneRoot.OnClickToBack_SELECT_STAFF -= ChangeStateToHireStaff;

        _sceneRoot.CloseSelectStaffPanel();
        _filterShopCasinoStaffActivatorProvider.ClearFailText();
    }

    private void ChangeStateToHireStaff()
    {
        _machineProvider.EnterState(_machineProvider.GetState<HireStaffState_Game>());
    }

    private void ChangeStateToMainState()
    {
        _sceneRoot.CloseBlackBackgroundPanel();

        _machineProvider.EnterState(_machineProvider.GetState<MainState_Game>());
    }

    private void ChangeStateToChooseSpotStaff()
    {
        _sceneRoot.CloseBlackBackgroundPanel();

        _machineProvider.EnterState(_machineProvider.GetState<ChooseSpotStaffState_Game>());
    }
}
