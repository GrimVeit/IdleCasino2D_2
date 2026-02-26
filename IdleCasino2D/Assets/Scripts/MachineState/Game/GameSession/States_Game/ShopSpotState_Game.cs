using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSpotState_Game : IState
{
    private readonly IStateMachineProvider _stateMachineProvider;
    private readonly IShopCasinoEntitySpotListener _shopCasinoEntitySpotListener;
    private readonly UIGameRoot _sceneRoot;

    public ShopSpotState_Game(IStateMachineProvider stateMachineProvider, IShopCasinoEntitySpotListener shopCasinoEntitySpotListener, UIGameRoot sceneRoot)
    {
        _stateMachineProvider = stateMachineProvider;
        _shopCasinoEntitySpotListener = shopCasinoEntitySpotListener;
        _sceneRoot = sceneRoot;
    }

    public void EnterState()
    {
        _sceneRoot.OnClickToBack_SHOP_SPOT += ActivateMainState;
        _shopCasinoEntitySpotListener.OnBuy += ActivateMainState;

        _sceneRoot.OpenAvatarBalancePanel();
        _sceneRoot.OpenBlackBackgroundPanel();
        _sceneRoot.OpenShopSpotPanel();
    }

    public void ExitState()
    {
        _sceneRoot.OnClickToBack_SHOP_SPOT -= ActivateMainState;
        _shopCasinoEntitySpotListener.OnBuy -= ActivateMainState;

        _sceneRoot.CloseBlackBackgroundPanel();
        _sceneRoot.CloseShopSpotPanel();
    }

    private void ActivateMainState()
    {
        _stateMachineProvider.EnterState(_stateMachineProvider.GetState<MainState_Game>());
    }
}
