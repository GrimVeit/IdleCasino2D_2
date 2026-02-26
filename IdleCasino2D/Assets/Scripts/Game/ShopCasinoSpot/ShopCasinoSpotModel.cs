using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCasinoSpotModel
{
    private readonly IListenerClickCasinoEntitySpot _listenerClickCasinoEntitySpot;
    private CasinoEntityClickInteractionAdapter _currentCasinoEntityAdapter;
    private ShopCasinoEntityDataSO _currentCasinoEntityDataSO;
    private readonly IMoneyProvider _moneyProvider;

    private readonly ShopCasinoEntityDatasSO _shopCasinoEntityDatasSO;

    private bool isListen = false;

    public ShopCasinoSpotModel(IListenerClickCasinoEntitySpot listenerClickCasinoEntitySpot, IMoneyProvider moneyProvider, ShopCasinoEntityDatasSO shopCasinoEntityDatasSO)
    {
        _listenerClickCasinoEntitySpot = listenerClickCasinoEntitySpot;
        _moneyProvider = moneyProvider;
        _shopCasinoEntityDatasSO = shopCasinoEntityDatasSO;

        _listenerClickCasinoEntitySpot.OnClickToCloseCasinoEntity += SetBuyEntity;
        _listenerClickCasinoEntitySpot.OnClickToOpenCasinoEntity += SetGame;
    }

    public void Initialize()
    {

    }

    public void Dispose()
    {
        _listenerClickCasinoEntitySpot.OnClickToCloseCasinoEntity -= SetBuyEntity;
    }

    public void Buy()
    {
        if (_moneyProvider.CanAfford(_currentCasinoEntityDataSO.Price))
        {
            _moneyProvider.SendMoney(-_currentCasinoEntityDataSO.Price);
            _currentCasinoEntityAdapter.CasinoEntityActivator.Open();

            OnBuy?.Invoke();
        }
    }

    private void SetGame(CasinoEntityClickInteractionAdapter casinoEntityAdapter)
    {
        if (!isListen) return;

        if (casinoEntityAdapter == null) return;

        casinoEntityAdapter.CasinoEntityManual.ManualStartGame();
    }

    private void SetBuyEntity(CasinoEntityClickInteractionAdapter casinoEntityAdapter)
    {
        if(!isListen) return;

        if(casinoEntityAdapter == null) return;

        _currentCasinoEntityDataSO = _shopCasinoEntityDatasSO.GetShopCasinoEntityData(casinoEntityAdapter.CasinoEntityType);
        if(_currentCasinoEntityDataSO == null) return;



        _currentCasinoEntityAdapter = casinoEntityAdapter;

        OnSetCasinoEntityData?.Invoke(_currentCasinoEntityDataSO);
    }

    public void ActivateListener()
    {
        isListen = true;
    }

    public void DeactivateListener()
    {
        isListen = false;
    }

    #region Output

    public event Action<ShopCasinoEntityDataSO> OnSetCasinoEntityData;

    public event Action OnBuy;

    #endregion
}
