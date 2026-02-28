using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilterShopCasinoStaffView : View
{
    [SerializeField] private TextMeshProUGUI textFail;
    [SerializeField] private Image imageStaff;
    [SerializeField] private TextMeshProUGUI textStaffName;

    public void SetTextFail(string fail)
    {
        textFail.text = fail;
    }

    public void SetData(ShopCasinoStaffData data)
    {
        imageStaff.sprite = data.Sprite;
        textStaffName.text = data.Name;
    }

    public void ClearFailText()
    {
        textFail.text = string.Empty;
    }
}
