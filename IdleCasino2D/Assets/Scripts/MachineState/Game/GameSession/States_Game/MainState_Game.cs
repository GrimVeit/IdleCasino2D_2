using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly IVisitorCounterTrafficProvider _visitorCounterTrafficProvider;
    private readonly ITouchCameraProvider _touchCameraProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly IClickDispatcherProvider _clickDispatcherProvider;
    private readonly IShopCasinoEntitySpotListener _shopCasinoEntitySpotListener;
    private readonly IShopCasinoEntitySpotProvider _shopCasinoEntitySpotProvider;

    private readonly IHostessEntityControllerProvider _hostessEntityControllerProvider;
    private readonly IHostessEntityControllerListener _hostessEntityControllerListener;

    public MainState_Game(IStateMachineProvider machineProvider, IVisitorCounterTrafficProvider visitorTrafficProvider, ITouchCameraProvider touchCameraProvider, UIGameRoot sceneRoot, IClickDispatcherProvider clickDispatcherProvider, IShopCasinoEntitySpotListener shopCasinoEntitySpotListener, IShopCasinoEntitySpotProvider shopCasinoEntitySpotProvider, IHostessEntityControllerProvider hostessEntityControllerProvider, IHostessEntityControllerListener hostessEntityControllerListener)
    {
        _machineProvider = machineProvider;
        _visitorCounterTrafficProvider = visitorTrafficProvider;
        _touchCameraProvider = touchCameraProvider;
        _sceneRoot = sceneRoot;
        _clickDispatcherProvider = clickDispatcherProvider;
        _shopCasinoEntitySpotListener = shopCasinoEntitySpotListener;
        _shopCasinoEntitySpotProvider = shopCasinoEntitySpotProvider;
        _hostessEntityControllerProvider = hostessEntityControllerProvider;
        _hostessEntityControllerListener = hostessEntityControllerListener;
    }

    public void EnterState()
    {
        _sceneRoot.OnCLickToUpgrade_MAIN += ChangeStateToUpgrade;
        _sceneRoot.OnClickToHireStaff_MAIN += ChangeStateToHireStaff;
        _shopCasinoEntitySpotListener.OnSetData += ChangeStateToShopSpot;
        _hostessEntityControllerListener.OnHostessOpenChoose += ChangeStateToChooseCasinoEntityState;

        _visitorCounterTrafficProvider.PlayTraffic();
        _touchCameraProvider.ActivateInteractive();
        _clickDispatcherProvider.Activate();
        _sceneRoot.OpenMainPanel();
        _sceneRoot.OpenAvatarBalancePanel();

        _shopCasinoEntitySpotProvider.ActivateListener();
        _hostessEntityControllerProvider.ActivateEntranceQueueInteractive();
    }

    public void ExitState()
    {
        _sceneRoot.OnCLickToUpgrade_MAIN -= ChangeStateToUpgrade;
        _sceneRoot.OnClickToHireStaff_MAIN -= ChangeStateToHireStaff;
        _shopCasinoEntitySpotListener.OnSetData -= ChangeStateToShopSpot;
        _hostessEntityControllerListener.OnHostessOpenChoose -= ChangeStateToChooseCasinoEntityState;

        _touchCameraProvider.DeactivateInteractive();
        _sceneRoot.CloseMainPanel();
        _clickDispatcherProvider.Deactivate();

        _shopCasinoEntitySpotProvider.DeactivateListener();
        _hostessEntityControllerProvider.DeactivateEntranceQueueInteractive();
    }

    private void ChangeStateToUpgrade()
    {
        _machineProvider.EnterState(_machineProvider.GetState<UpgradeState_Game>());
    }

    private void ChangeStateToHireStaff()
    {
        _machineProvider.EnterState(_machineProvider.GetState<HireStaffState_Game>());
    }

    private void ChangeStateToShopSpot()
    {
        _machineProvider.EnterState(_machineProvider.GetState<ShopSpotState_Game>());
    }

    private void ChangeStateToChooseCasinoEntityState()
    {
        _machineProvider.EnterState(_machineProvider.GetState<ChooseCasinoEntityState_Game>());
    }
}
