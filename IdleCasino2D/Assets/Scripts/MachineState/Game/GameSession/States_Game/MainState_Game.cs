using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainState_Game : IState
{
    private IStateMachineProvider _machineProvider;
    private readonly IVisitorCounterTrafficProvider _visitorCounterTrafficProvider;

    public MainState_Game(IStateMachineProvider machineProvider, IVisitorCounterTrafficProvider visitorTrafficProvider)
    {
        _machineProvider = machineProvider;
        _visitorCounterTrafficProvider = visitorTrafficProvider;
    }

    public void EnterState()
    {
        _visitorCounterTrafficProvider.PlayTraffic();
    }

    public void ExitState()
    {

    }
}
