using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameAndAvatarInputState_Menu : IState
{
    private readonly IStateMachineProvider _globalStateMachineProvider;
    private readonly UIMainMenuRoot _sceneRoot;
    private readonly NicknamePresenter _nicknamePresenter;
    private readonly AvatarPresenter _avatarPresenter;

    public NameAndAvatarInputState_Menu(IStateMachineProvider globalStateMachineProvider, UIMainMenuRoot sceneRoot, NicknamePresenter nicknamePresenter, AvatarPresenter avatarPresenter)
    {
        _globalStateMachineProvider = globalStateMachineProvider;
        _sceneRoot = sceneRoot;
        _nicknamePresenter = nicknamePresenter;
        _avatarPresenter = avatarPresenter;
    }

    public void EnterState()
    {
        Debug.Log("<color=red>ACTIVATE STATE - NAME AND AVATAR INPUT STATE / MENU</color>");

        //_nicknamePresenter.OnChooseNickname += _firebaseAuthenticationPresenter.SetNickname;
        //_nicknamePresenter.OnChooseNickname += _firebaseDatabasePresenter.SetNickname;
        //_avatarPresenter.OnChooseAvatar += _firebaseDatabasePresenter.SetAvatar;

        //_sceneRoot.OnClickToRegistrate_Registration += ChangeStateToRegistration;

        //_sceneRoot.OpenRegistrationPanel();
    }

    public void ExitState()
    {
        //_nicknamePresenter.OnChooseNickname -= _firebaseAuthenticationPresenter.SetNickname;
        //_nicknamePresenter.OnChooseNickname -= _firebaseDatabasePresenter.SetNickname;
        //_avatarPresenter.OnChooseAvatar -= _firebaseDatabasePresenter.SetAvatar;

        //_sceneRoot.OnClickToRegistrate_Registration -= ChangeStateToRegistration;

        //_sceneRoot.CloseRegistrationPanel();
    }

    private void ChangeStateToRegistration()
    {
        _globalStateMachineProvider.EnterState(_globalStateMachineProvider.GetState<RegistrationState_Menu>());
    }
}
