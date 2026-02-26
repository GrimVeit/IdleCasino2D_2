using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopCasinoEntityDataSO", menuName = "Game/Shop/CasinoEntityData")]
public class ShopCasinoEntityDataSO : ScriptableObject
{
    public CasinoEntityType CasinoEntityType => casinoEntityType;
    public string Name => casinoName;
    public int Price => shopPrice;
    public Sprite Sprite => shopSprite;

    [SerializeField] private CasinoEntityType casinoEntityType;
    [SerializeField] private string casinoName;
    [SerializeField] private int shopPrice;
    [SerializeField] private Sprite shopSprite;
}
