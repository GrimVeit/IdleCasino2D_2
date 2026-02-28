using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseSpotStaffState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly IFilterShopCasinoStaffProvider _filterShopCasinoStaffActivatorProvider;
    private readonly IFilterShopCasinoStaffListener _filterShopCasinoStaffListener;
    private readonly ITouchCameraProvider _touchCameraProvider;
    private readonly IClickDispatcherProvider _clickDispatcherProvider;

    public ChooseSpotStaffState_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot, IFilterShopCasinoStaffProvider filterShopCasinoStaffActivatorProvider, IFilterShopCasinoStaffListener filterShopCasinoStaffListener, ITouchCameraProvider touchCameraProvider, IClickDispatcherProvider clickDispatcherProvider)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
        _filterShopCasinoStaffActivatorProvider = filterShopCasinoStaffActivatorProvider;
        _filterShopCasinoStaffListener = filterShopCasinoStaffListener;
        _touchCameraProvider = touchCameraProvider;
        _clickDispatcherProvider = clickDispatcherProvider;
    }

    public void EnterState()
    {
        _sceneRoot.OnClickToBack_CHOOSE_AVAILABLE_SPOT += SetStateToMain;
        _filterShopCasinoStaffListener.OnStaffPurchased += SetStateToMain;

        _sceneRoot.OpenChooseAvailableStaffPanel();
        _filterShopCasinoStaffActivatorProvider.Activate();
        _touchCameraProvider.ActivateInteractive();
        _clickDispatcherProvider.Activate();
    }

    public void ExitState()
    {
        _sceneRoot.OnClickToBack_CHOOSE_AVAILABLE_SPOT -= SetStateToMain;
        _filterShopCasinoStaffListener.OnStaffPurchased -= SetStateToMain;

        _sceneRoot.CloseChooseAvailableStaffPanel();
        _filterShopCasinoStaffActivatorProvider.Deactivate();
    }

    private void SetStateToMain()
    {
        _filterShopCasinoStaffActivatorProvider.CancelSelection();

        _machineProvider.EnterState(_machineProvider.GetState<MainState_Game>());
    }
}
