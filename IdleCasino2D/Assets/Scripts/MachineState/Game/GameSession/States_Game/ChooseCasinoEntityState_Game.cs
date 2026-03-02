using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCasinoEntityState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly IHostessEntityControllerListener _hostessEntityControllerListener;
    private readonly IHostessEntityControllerProvider _hostessEntityControllerProvider;

    private readonly ITouchCameraProvider _touchCameraProvider;
    private readonly IClickDispatcherProvider _clickDispatcherProvider;

    public ChooseCasinoEntityState_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot, IHostessEntityControllerListener hostessEntityControllerListener, IHostessEntityControllerProvider hostessEntityControllerProvider, ITouchCameraProvider touchCameraProvider, IClickDispatcherProvider clickDispatcherProvider)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
        _hostessEntityControllerListener = hostessEntityControllerListener;
        _hostessEntityControllerProvider = hostessEntityControllerProvider;
        _touchCameraProvider = touchCameraProvider;
        _clickDispatcherProvider = clickDispatcherProvider;
    }

    public void EnterState()
    {
        _hostessEntityControllerListener.OnSuccessAssign += ChangeStateToMain;
        _hostessEntityControllerListener.OnLeave += ChangeStateToMain;
        _sceneRoot.OnClickToBack_CHOOSE_AVAILABLE_ENTITY += ChangeStateToMain;

        _hostessEntityControllerProvider.ActivateInteractiveCasinoEntity();
        _sceneRoot.OpenChooseAvailableSpotPanel();
        _sceneRoot.OpenLeavePanel();

        _touchCameraProvider.ActivateInteractive();
        _clickDispatcherProvider.Activate();
    }

    public void ExitState()
    {
        _hostessEntityControllerListener.OnSuccessAssign -= ChangeStateToMain;
        _hostessEntityControllerListener.OnLeave -= ChangeStateToMain;
        _sceneRoot.OnClickToBack_CHOOSE_AVAILABLE_ENTITY -= ChangeStateToMain;

        _hostessEntityControllerProvider.DeactivateInteractiveCasinoEntity();
        _sceneRoot.CloseChooseAvailableSpotPanel();
        _sceneRoot.CloseLeavePanel();
        _hostessEntityControllerProvider.ActivateAll();
    }

    private void ChangeStateToMain()
    {
        _machineProvider.EnterState(_machineProvider.GetState<MainState_Game>());
    }
}
