using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCasinoPersonalModel
{
    private readonly ShopCasinoPersonalDatasSO _shopCasinoPersonalDatasSO;
    private readonly IMoneyProvider _moneyProvider;

    private ShopCasinoPersonalDataGroup _currentShopPersonalGroup;
    private int _currentSkinId = 0;

    public ShopCasinoPersonalModel(ShopCasinoPersonalDatasSO shopCasinoPersonalDatasSO, IMoneyProvider moneyProvider)
    {
        _shopCasinoPersonalDatasSO = shopCasinoPersonalDatasSO;
        _moneyProvider = moneyProvider;
    }

    public void SetShopPersonalGroup(PersonalType personalType)
    {
        _currentShopPersonalGroup = _shopCasinoPersonalDatasSO.GetDataGroup(personalType);
        _currentSkinId = 0;

        if(_currentShopPersonalGroup == null)
        {
            Debug.LogWarning("Not found Shop Personal Group with PersonalType." + personalType);
            return;
        }

        OnChooseShopPersonalGroup?.Invoke(_currentShopPersonalGroup);
        OnChooseSkinId?.Invoke(_currentSkinId);
    }

    public void SetSkinId(int skinId)
    {
        if(_currentSkinId == skinId) return;

        OnUnchooseSkinId?.Invoke(_currentSkinId);

        _currentSkinId = skinId;
        OnChooseSkinId?.Invoke(_currentSkinId);
    }

    #region Output

    public event Action<ShopCasinoPersonalDataGroup> OnChooseShopPersonalGroup;

    public event Action<int> OnChooseSkinId;
    public event Action<int> OnUnchooseSkinId;

    #endregion
}
