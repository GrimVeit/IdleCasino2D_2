using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistrationState_Menu : IState
{
    private readonly IStateMachineProvider _globalStateMachineProvider;
    private readonly UIMainMenuRoot _sceneRoot;

    public RegistrationState_Menu(IStateMachineProvider globalStateMachineProvider, UIMainMenuRoot sceneRoot)
    {
        _globalStateMachineProvider = globalStateMachineProvider;
        _sceneRoot = sceneRoot;
    }

    public void EnterState()
    {
        Debug.Log("<color=red>ACTIVATE STATE - REGISTRATION STATE / MENU</color>");

        //_firebaseAuthenticationPresenter.OnSignUp += _firebaseDatabasePresenter.CreateEmptyDataToServer;
        //_firebaseAuthenticationPresenter.OnSignUp += ChangeStateToStartMainMenu;

        //_firebaseAuthenticationPresenter.OnSignUpError += ChangeStateToNameAndAvatarInput;

        //_firebaseAuthenticationPresenter.SignUp();

        //_sceneRoot.OpenLoadRegistrationPanel();
    }

    public void ExitState()
    {
        //_firebaseAuthenticationPresenter.OnSignUp -= _firebaseDatabasePresenter.CreateEmptyDataToServer;
        //_firebaseAuthenticationPresenter.OnSignUp -= ChangeStateToStartMainMenu;

        //_firebaseAuthenticationPresenter.OnSignUpError -= ChangeStateToNameAndAvatarInput;

        //_sceneRoot.CloseLoadRegistrationPanel();
    }

    private void ChangeStateToNameAndAvatarInput()
    {
        _globalStateMachineProvider.EnterState(_globalStateMachineProvider.GetState<NameAndAvatarInputState_Menu>());
    }

    private void ChangeStateToStartMainMenu()
    {
        _globalStateMachineProvider.EnterState(_globalStateMachineProvider.GetState<StartMainState_Menu>());
    }
}
