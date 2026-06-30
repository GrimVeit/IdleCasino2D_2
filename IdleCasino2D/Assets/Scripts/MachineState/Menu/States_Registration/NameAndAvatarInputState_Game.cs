using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameAndAvatarInputState_Game : IState
{
    private readonly IStateMachineProvider _globalStateMachineProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly NicknamePresenter _nicknamePresenter;
    private readonly FirebaseAuthenticationPresenter _firebaseAuthenticationPresenter;
    private readonly FirebaseDatabasePresenter _firebaseDatabasePresenter;

    public NameAndAvatarInputState_Game(IStateMachineProvider globalStateMachineProvider, UIGameRoot sceneRoot, NicknamePresenter nicknamePresenter, FirebaseAuthenticationPresenter firebaseAuthenticationPresenter, FirebaseDatabasePresenter firebaseDatabasePresenter)
    {
        _globalStateMachineProvider = globalStateMachineProvider;
        _sceneRoot = sceneRoot;
        _nicknamePresenter = nicknamePresenter;
        _firebaseAuthenticationPresenter = firebaseAuthenticationPresenter;
        _firebaseDatabasePresenter = firebaseDatabasePresenter;
    }

    public void EnterState()
    {
        Debug.Log("<color=red>ACTIVATE STATE - NAME INPUT STATE / GAME</color>");

        _nicknamePresenter.OnChooseNickname += _firebaseAuthenticationPresenter.SetNickname;
        _nicknamePresenter.OnChooseNickname += _firebaseDatabasePresenter.SetNickname;

        _sceneRoot.OnClickToRegistrate_Registration += ChangeStateToRegistration;

        _sceneRoot.OpenRegistrationPanel();
    }

    public void ExitState()
    {
        _nicknamePresenter.OnChooseNickname -= _firebaseAuthenticationPresenter.SetNickname;
        _nicknamePresenter.OnChooseNickname -= _firebaseDatabasePresenter.SetNickname;

        _sceneRoot.OnClickToRegistrate_Registration -= ChangeStateToRegistration;

        _sceneRoot.CloseRegistrationPanel();
    }

    private void ChangeStateToRegistration()
    {
        _globalStateMachineProvider.EnterState(_globalStateMachineProvider.GetState<RegistrationState_Game>());
    }
}
