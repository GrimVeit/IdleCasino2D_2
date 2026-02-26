using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCasinoSpotView : View
{
    [SerializeField] private ShopCasinoSpotSizes shopCasinoSpotSizes;
    [SerializeField] private RectTransform transformSprite;
    [SerializeField] private TextMeshProUGUI textSpotName;
    [SerializeField] private TextMeshProUGUI textSpotPrice;
    [SerializeField] private Image imageSpot;
    [SerializeField] private Button buttonBuySpot;

    public void Initialize()
    {
        buttonBuySpot.onClick.AddListener(ClickToBuySpot);
    }

    public void Dispose()
    {
        buttonBuySpot.onClick.RemoveListener(ClickToBuySpot);
    }

    public void SetSpotData(ShopCasinoEntityDataSO dataSO)
    {
        textSpotName.text = dataSO.Name;
        textSpotPrice.text = $"{dataSO.Price}$";
        imageSpot.sprite = dataSO.Sprite;

        transformSprite.sizeDelta = shopCasinoSpotSizes.GetSize(dataSO.CasinoEntityType).Size;
    }

    #region Output

    public event Action OnClickToBuySpot;

    private void ClickToBuySpot()
    {
        OnClickToBuySpot?.Invoke();
    }

    #endregion
}

[Serializable]
public class ShopCasinoSpotSizes
{
    [SerializeField] private List<ShopCasinoSpotSize> shopCasinoSpotSizes;

    public ShopCasinoSpotSize GetSize(CasinoEntityType entityType)
    {
        return shopCasinoSpotSizes.Find(data => data.EntityType == entityType);
    }
}

[Serializable]
public class ShopCasinoSpotSize
{
    [SerializeField] private CasinoEntityType entityType;
    [SerializeField] private Vector2 vectorSize;

    public Vector2 Size;
    public CasinoEntityType EntityType => entityType;
}
