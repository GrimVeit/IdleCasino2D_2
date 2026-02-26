using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopCasinoEntityDatasSO", menuName = "Game/Shop/CasinoEntityDatas")]
public class ShopCasinoEntityDatasSO : ScriptableObject
{
    [SerializeField] private List<ShopCasinoEntityDataSO> datas = new();

    public ShopCasinoEntityDataSO GetShopCasinoEntityData(CasinoEntityType type)
    {
        return datas.Find(scod => scod.CasinoEntityType == type);
    }
}
