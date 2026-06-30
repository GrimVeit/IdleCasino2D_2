using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistrationState_Game : IState
{
    private readonly IStateMachineProvider _globalStateMachineProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly FirebaseAuthenticationPresenter _firebaseAuthenticationPresenter;
    private readonly FirebaseDatabasePresenter _firebaseDatabasePresenter;

    public RegistrationState_Game(IStateMachineProvider globalStateMachineProvider, UIGameRoot sceneRoot, FirebaseAuthenticationPresenter firebaseAuthenticationPresenter, FirebaseDatabasePresenter firebaseDatabasePresenter)
    {
        _globalStateMachineProvider = globalStateMachineProvider;
        _sceneRoot = sceneRoot;
        _firebaseAuthenticationPresenter = firebaseAuthenticationPresenter;
        _firebaseDatabasePresenter = firebaseDatabasePresenter;
    }

    public void EnterState()
    {
        Debug.Log("<color=red>ACTIVATE STATE - REGISTRATION STATE / GAME</color>");

        _firebaseAuthenticationPresenter.OnSignUp += _firebaseDatabasePresenter.CreateEmptyDataToServer;
        _firebaseAuthenticationPresenter.OnSignUp += ChangeStateToStartMainMenu;

        _firebaseAuthenticationPresenter.OnSignUpError += ChangeStateToNameAndAvatarInput;

        _firebaseAuthenticationPresenter.SignUp();

        _sceneRoot.OpenLoadingRegistrationPanel();
    }

    public void ExitState()
    {
        _firebaseAuthenticationPresenter.OnSignUp -= _firebaseDatabasePresenter.CreateEmptyDataToServer;
        _firebaseAuthenticationPresenter.OnSignUp -= ChangeStateToStartMainMenu;

        _firebaseAuthenticationPresenter.OnSignUpError -= ChangeStateToNameAndAvatarInput;

        _sceneRoot.CloseLoadingRegistrationPanel();
    }

    private void ChangeStateToNameAndAvatarInput()
    {
        _globalStateMachineProvider.EnterState(_globalStateMachineProvider.GetState<NameAndAvatarInputState_Game>());
    }

    private void ChangeStateToStartMainMenu()
    {
        _globalStateMachineProvider.EnterState(_globalStateMachineProvider.GetState<StartState_Game>());
    }
}
