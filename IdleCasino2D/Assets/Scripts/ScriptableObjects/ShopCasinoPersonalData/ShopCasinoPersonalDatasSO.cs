using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopCasinoPersonalDatasSO", menuName = "Game/Shop/CasinoPersonalDatas")]
public class ShopCasinoPersonalDatasSO : ScriptableObject
{
    [SerializeField] private List<ShopCasinoPersonalDataGroup> shopCasinoPersonalDataGroups = new();

    public ShopCasinoPersonalDataGroup GetDataGroup(PersonalType personalType)
    {
        return shopCasinoPersonalDataGroups.Find(data => data.PersonalType == personalType);
    }
}

[System.Serializable]
public class ShopCasinoPersonalDataGroup
{
    [SerializeField] private PersonalType personalType;
    [SerializeField] private string name;
    [SerializeField] private int price;
    [SerializeField] private List<ShopCasinoPersonalData> shopCasinoPersonalDatas = new();

    public PersonalType PersonalType => personalType;
    public string Name => name;
    public int Price => price;
    public List<ShopCasinoPersonalData> ShopCasinoPersonalDatas => shopCasinoPersonalDatas;
}

[System.Serializable]
public class ShopCasinoPersonalData
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private int skinId;

    public Sprite SpriteSkin => sprite;
    public int SkinId => skinId;
}
