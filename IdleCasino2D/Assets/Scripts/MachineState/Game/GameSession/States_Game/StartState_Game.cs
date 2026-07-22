using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartState_Game : IState
{
    private readonly IStateMachineProvider _machineProvider;
    private readonly UIGameRoot _sceneRoot;
    private readonly FirebaseAuthenticationPresenter _firebaseAuthenticationPresenter;
    private readonly FirebaseDatabasePresenter _firebaseDatabasePresenter;
    private readonly IStoreFirstLaunchProvider _storeFirstLaunchProvider;

    private IEnumerator timer;

    public StartState_Game(IStateMachineProvider machineProvider, UIGameRoot sceneRoot, FirebaseAuthenticationPresenter firebaseAuthenticationPresenter, FirebaseDatabasePresenter firebaseDatabasePresenter, IStoreFirstLaunchProvider storeFirstLaunchProvider)
    {
        _machineProvider = machineProvider;
        _sceneRoot = sceneRoot;
        _firebaseAuthenticationPresenter = firebaseAuthenticationPresenter;
        _firebaseDatabasePresenter = firebaseDatabasePresenter;
        _storeFirstLaunchProvider = storeFirstLaunchProvider;
    }

    public void EnterState()
    {
        _sceneRoot.OnClickToPlay_START += Timer;

        if (_firebaseAuthenticationPresenter.IsAuthorization())
        {
            _firebaseDatabasePresenter.SaveChangeToServer();
            _firebaseDatabasePresenter.DisplayUsersRecords();
        }

        _sceneRoot.OpenStartPanel();
    }

    public void ExitState()
    {
        if (timer != null) Coroutines.Stop(timer);

        _sceneRoot.OnClickToPlay_START -= Timer;

        _sceneRoot.ClosePurpleBackgroundPanel();
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

        CheckTutorial();
    }

    private void CheckTutorial()
    {
        if (!_storeFirstLaunchProvider.IsFirstLaunch)
        {
            ActivateMainState();
        }
        else
        {
            ActivateTutorialState();
        }
    }

    private void ActivateMainState()
    {
        _machineProvider.EnterState(_machineProvider.GetState<CheckProfitOnlineState_Game>());
    }

    private void ActivateTutorialState()
    {
        _machineProvider.EnterState(_machineProvider.GetState<Tutorial01_Welcome_State_Game>());
    }
}
