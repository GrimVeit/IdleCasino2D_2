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

    public MainState_Game(IStateMachineProvider machineProvider, IVisitorCounterTrafficProvider visitorTrafficProvider, ITouchCameraProvider touchCameraProvider, UIGameRoot sceneRoot, IClickDispatcherProvider clickDispatcherProvider)
    {
        _machineProvider = machineProvider;
        _visitorCounterTrafficProvider = visitorTrafficProvider;
        _touchCameraProvider = touchCameraProvider;
        _sceneRoot = sceneRoot;
        _clickDispatcherProvider = clickDispatcherProvider;
    }

    public void EnterState()
    {
        _sceneRoot.OnCLickToUpgrade_MAIN += ChangeStateToUpgrade;
        _sceneRoot.OnClickToHireStaff_MAIN += ChangeStateToHireStaff;

        _visitorCounterTrafficProvider.PlayTraffic();
        _touchCameraProvider.ActivateInteractive();
        _clickDispatcherProvider.Activate();
        _sceneRoot.OpenMainPanel();
        _sceneRoot.OpenAvatarBalancePanel();
    }

    public void ExitState()
    {
        _sceneRoot.OnCLickToUpgrade_MAIN -= ChangeStateToUpgrade;
        _sceneRoot.OnClickToHireStaff_MAIN -= ChangeStateToHireStaff;

        _touchCameraProvider.DeactivateInteractive();
        _sceneRoot.CloseMainPanel();
        _clickDispatcherProvider.Deactivate();
    }

    private void ChangeStateToUpgrade()
    {
        _machineProvider.EnterState(_machineProvider.GetState<UpgradeState_Game>());
    }

    private void ChangeStateToHireStaff()
    {
        _machineProvider.EnterState(_machineProvider.GetState<HireStaffState_Game>());
    }
}
