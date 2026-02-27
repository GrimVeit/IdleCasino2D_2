using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCasinoPersonalView : View
{
    [SerializeField] private List<ShopCasinoPersonalChoose> shopCasinoPersonalChooses = new List<ShopCasinoPersonalChoose>();

    [SerializeField] private TextMeshProUGUI textPersonal;
    [SerializeField] private ShopCasinoPersonalVisual personalVisualPrefab;
    [SerializeField] private Transform transformContent;

    private readonly List<ShopCasinoPersonalVisual> _shopCasinoPersonalVisuals = new();

    public void Initialize()
    {
        for (int i = 0; i < shopCasinoPersonalChooses.Count; i++)
        {
            shopCasinoPersonalChooses[i].OnChoosePersonalType += ChoosePersonalType;
            shopCasinoPersonalChooses[i].Initialize();
        }
    }

    public void Dispose()
    {
        for (int i = 0; i < shopCasinoPersonalChooses.Count; i++)
        {
            shopCasinoPersonalChooses[i].OnChoosePersonalType -= ChoosePersonalType;
            shopCasinoPersonalChooses[i].Dispose();
        }
    }

    public void SetDataGroup(ShopCasinoPersonalDataGroup personalDataGroup)
    {
        textPersonal.text = personalDataGroup.Name;

        for (int i = 0; i < _shopCasinoPersonalVisuals.Count; i++)
        {
            _shopCasinoPersonalVisuals[i].OnChoosePersonal -= ChoosePersonalSkinId;
        }

        _shopCasinoPersonalVisuals.Clear();

        foreach (Transform child in transformContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < personalDataGroup.ShopCasinoPersonalDatas.Count; i++)
        {
            var visual = Instantiate(personalVisualPrefab, transformContent);
            visual.SetData(personalDataGroup.ShopCasinoPersonalDatas[i]);
            visual.OnChoosePersonal += ChoosePersonalSkinId;

            _shopCasinoPersonalVisuals.Add(visual);
        }
    }

    public void Choose(int skinId)
    {
        var visual = GetShopCasinoPersonalVisual(skinId);

        if(visual == null)
        {
            Debug.Log("Not found ShopCasinoPersonalVisual with SkinId - " + skinId);
            return;
        }

        visual.Choose();
    }

    public void Unchoose(int skinId)
    {
        var visual = GetShopCasinoPersonalVisual(skinId);

        if (visual == null)
        {
            Debug.Log("Not found ShopCasinoPersonalVisual with SkinId - " + skinId);
            return;
        }

        visual.Unchoose();
    }

    private ShopCasinoPersonalVisual GetShopCasinoPersonalVisual(int skinId)
    {
        return _shopCasinoPersonalVisuals.FirstOrDefault(s => s.SkinId == skinId);
    }

    #region Output

    public event Action<StaffType> OnChoosePersonalType;
    public event Action<int> OnChoosePersonalSkinId;

    private void ChoosePersonalType(StaffType type)
    {
        OnChoosePersonalType?.Invoke(type);
    }

    private void ChoosePersonalSkinId(int skinId)
    {
        OnChoosePersonalSkinId?.Invoke(skinId);
    }

    #endregion
}

[Serializable]
public class ShopCasinoPersonalChoose
{
    [SerializeField] private StaffType personalType;
    [SerializeField] private Button buttonChoose;

    public void Initialize()
    {
        buttonChoose.onClick.AddListener(Choose);
    }

    public void Dispose()
    {
        buttonChoose.onClick.RemoveListener(Choose);
    }

    #region Output

    public event Action<StaffType> OnChoosePersonalType;

    private void Choose()
    {
        OnChoosePersonalType?.Invoke(personalType);
    }

    #endregion
}
