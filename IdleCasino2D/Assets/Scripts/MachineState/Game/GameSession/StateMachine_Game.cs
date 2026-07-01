using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine_Game : IStateMachineProvider
{
    private readonly Dictionary<Type, IState> states = new();

    private IState _currentState;

    public StateMachine_Game
        (
        UIGameRoot sceneRoot,
        IVisitorCounterTrafficProvider visitorCounterTrafficProvider,
        ITouchCameraProvider touchCameraProvider,
        IClickDispatcherProvider clickDispatcherProvider,
        IShopCasinoEntitySpotListener shopCasinoEntitySpotListener,
        IShopCasinoEntitySpotProvider shopCasinoEntitySpotProvider,
        IShopCasinoPersonalListener shopCasinoPersonalListener,
        IFilterShopCasinoStaffProvider filterShopCasinoStaffActivatorProvider,
        IFilterShopCasinoStaffListener filterShopCasinoStaffListener,
        IHostessEntityControllerListener hostessEntityControllerListener,
        IHostessEntityControllerProvider hostessEntityControllerProvider,
        ICasinoProfitListener casinoProfitListener,
        IAdministratorVisualProvider administratorVisualProvider,
        IProfitOfflineInfo profitOfflineInfo,
        IProfitOfflineListener profitOfflineListener,
        ISoundProvider soundProvider,
        FirebaseAuthenticationPresenter firebaseAuthenticationPresenter,
        FirebaseDatabasePresenter firebaseDatabasePresenter,
        NicknamePresenter nicknamePresenter
        )
    {
        states[typeof(CheckAuthorizationState_Game)] = new CheckAuthorizationState_Game(this, firebaseAuthenticationPresenter, sceneRoot);
        states[typeof(NameAndAvatarInputState_Game)] = new NameAndAvatarInputState_Game(this, sceneRoot, nicknamePresenter, firebaseAuthenticationPresenter, firebaseDatabasePresenter);
        states[typeof(RegistrationState_Game)] = new RegistrationState_Game(this, sceneRoot, firebaseAuthenticationPresenter, firebaseDatabasePresenter);




        states[typeof(StartState_Game)] = new StartState_Game(this, sceneRoot);
        states[typeof(MainState_Game)] = new MainState_Game(this, visitorCounterTrafficProvider, touchCameraProvider, sceneRoot, clickDispatcherProvider, shopCasinoEntitySpotListener, shopCasinoEntitySpotProvider, hostessEntityControllerProvider, hostessEntityControllerListener, administratorVisualProvider);
        states[typeof(ChooseCasinoEntityState_Game)] = new ChooseCasinoEntityState_Game(this, sceneRoot, hostessEntityControllerListener, hostessEntityControllerProvider, touchCameraProvider, clickDispatcherProvider);
        states[typeof(HireStaffState_Game)] = new HireStaffState_Game(this, sceneRoot, shopCasinoPersonalListener);

        states[typeof(LeaderboardState_Game)] = new LeaderboardState_Game(this, sceneRoot);

        states[typeof(CheckProfitOnlineState_Game)] = new CheckProfitOnlineState_Game(this, profitOfflineInfo, soundProvider);
        states[typeof(ProfitOnlineState_Game)] = new ProfitOnlineState_Game(this, profitOfflineListener, sceneRoot);

        states[typeof(UpgradeState_Game)] = new UpgradeState_Game(this, sceneRoot, casinoProfitListener);
        states[typeof(ProfitUpgradeState_Game)] = new ProfitUpgradeState_Game(this, sceneRoot);

        states[typeof(ShopSpotState_Game)] = new ShopSpotState_Game(this, shopCasinoEntitySpotListener, sceneRoot);

        states[typeof(SelectStaffState_Game)] = new SelectStaffState_Game(this, sceneRoot, filterShopCasinoStaffActivatorProvider, filterShopCasinoStaffListener);
        states[typeof(ChooseSpotStaffState_Game)] = new ChooseSpotStaffState_Game(this, sceneRoot, filterShopCasinoStaffActivatorProvider, filterShopCasinoStaffListener, touchCameraProvider, clickDispatcherProvider);
    }

    public void Initialize()
    {
        EnterState(GetState<CheckAuthorizationState_Game>());
    }

    public void Dispose()
    {
        _currentState?.ExitState();
    }

    public IState GetState<T>() where T : IState
    {
        return states[typeof(T)];
    }

    public void EnterState(IState state)
    {
        _currentState?.ExitState();

        _currentState = state;
        _currentState.EnterState();
    }
}
