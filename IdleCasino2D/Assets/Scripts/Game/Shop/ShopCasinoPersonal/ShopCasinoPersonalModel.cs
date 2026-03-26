using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCasinoPersonalModel
{
    private readonly ShopCasinoPersonalDatasSO _shopCasinoPersonalDatasSO;
    private readonly ISoundProvider _soundProvider;

    private ShopCasinoPersonalDataGroup _currentShopPersonalGroup;
    private int _currentSkinId = 0;

    public ShopCasinoPersonalModel(ShopCasinoPersonalDatasSO shopCasinoPersonalDatasSO, ISoundProvider soundProvider)
    {
        _shopCasinoPersonalDatasSO = shopCasinoPersonalDatasSO;
        _soundProvider = soundProvider;
    }

    public void SetShopPersonalGroup(StaffType personalType)
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

        _soundProvider.PlayOneShot("PanelOpen");
    }

    public void SetSkinId(int skinId)
    {
        if(_currentSkinId == skinId) return;

        OnUnchooseSkinId?.Invoke(_currentSkinId);

        _currentSkinId = skinId;
        OnChooseSkinId?.Invoke(_currentSkinId);
    }

    public void SubmitChoice()
    {
        OnChooseStaffData?.Invoke(new ShopCasinoStaffData
            (_currentShopPersonalGroup.ShopCasinoPersonalDatas[_currentSkinId].SkinId,
            _currentShopPersonalGroup.Name,
            _currentShopPersonalGroup.ShopCasinoPersonalDatas[_currentSkinId].SpriteSkin,
            _currentShopPersonalGroup.PersonalType,
            _currentShopPersonalGroup.Price));
    }

    #region Output

    public event Action<ShopCasinoPersonalDataGroup> OnChooseShopPersonalGroup;
    public event Action<ShopCasinoStaffData> OnChooseStaffData;

    public event Action<int> OnChooseSkinId;
    public event Action<int> OnUnchooseSkinId;

    #endregion
}

public class ShopCasinoStaffData
{
    public int SkinId { get; }
    public string Name { get; }
    public Sprite Sprite { get; }
    public StaffType StaffType { get; }
    public int Price { get; }

    public ShopCasinoStaffData(int skinId, string name, Sprite sprite, StaffType staffType, int price)
    {
        SkinId = skinId;
        Name = name;
        Sprite = sprite;
        StaffType = staffType;
        Price = price;
    }
}
