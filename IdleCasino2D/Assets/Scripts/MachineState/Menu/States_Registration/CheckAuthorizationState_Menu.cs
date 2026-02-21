using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAuthorizationState_Menu : IState
{
    private readonly IStateMachineProvider _machineProvider;

    public CheckAuthorizationState_Menu(IStateMachineProvider machineProvider)
    {
        _machineProvider = machineProvider;
    }

    public void EnterState()
    {
        Debug.Log("<color=red>ACTIVATE STATE - AUTHORIZATION STATE / MENU</color>");

        //if (_firebaseAuthentication.IsAuthorization())
        //{
        //    ChangeStateToStartMain();
        //}
        //else
        //{
        //    ChangeStateToStartRegistration();
        //}
    }

    public void ExitState()
    {

    }

    private void ChangeStateToStartRegistration()
    {
        _machineProvider.EnterState(_machineProvider.GetState<NameAndAvatarInputState_Menu>());
    }

    private void ChangeStateToStartMain()
    {
        _machineProvider.EnterState(_machineProvider.GetState<StartMainState_Menu>());
    }
}
