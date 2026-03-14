using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;

    private IEnumerator timer;

    public StartState_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
    }

    public void EnterState()
    {
        _sceneRoot.OnClickToPlay_START += Timer;

        _sceneRoot.OpenStartPanel();
    }

    public void ExitState()
    {
        if (timer != null) Coroutines.Stop(timer);

        _sceneRoot.OnClickToPlay_START -= Timer;

        _sceneRoot.CloseStartPanel();
    }

    public void Timer()
    {
        if(timer != null) Coroutines.Stop(timer);

        timer = TimerCoro();
        Coroutines.Start(timer);
    }

    private IEnumerator TimerCoro()
    {
        yield return new WaitForSeconds(0.3f);

        ActivateMainState();
    }

    private void ActivateMainState()
    {
        _machineProvider.EnterState(_machineProvider.GetState<CheckProfitOnlineState_Game>());
    }
}
