using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachineProvider
{
    public IState GetState<T>() where T : IState;

    public void EnterState(IState state);
}
