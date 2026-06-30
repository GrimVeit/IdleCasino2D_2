using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAuthorizationState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly FirebaseAuthenticationPresenter _firebaseAuthenticationPresenter;

    public CheckAuthorizationState_Game(IStateMachineProvider machineProvider, FirebaseAuthenticationPresenter firebaseAuthenticationPresenter)
    {
        _machineProvider = machineProvider;
        _firebaseAuthenticationPresenter = firebaseAuthenticationPresenter;
    }

    public void EnterState()
    {
        Debug.Log("<color=red>ACTIVATE STATE - AUTHORIZATION STATE / GAME</color>");

        if (_firebaseAuthenticationPresenter.IsAuthorization())
        {
            ChangeStateToStartMain();
        }
        else
        {
            ChangeStateToStartRegistration();
        }
    }

    public void ExitState()
    {

    }

    private void ChangeStateToStartRegistration()
    {
        _machineProvider.EnterState(_machineProvider.GetState<NameAndAvatarInputState_Game>());
    }

    private void ChangeStateToStartMain()
    {
        _machineProvider.EnterState(_machineProvider.GetState<StartState_Game>());
    }
}
