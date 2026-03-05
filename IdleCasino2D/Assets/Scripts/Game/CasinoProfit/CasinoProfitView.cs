using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CasinoProfitView : View
{
    [SerializeField] private List<CasinoProfitCasinoType> casinoProfitCasinoTypes = new();
    [SerializeField] private List<CasinoProfitInfo> casinoProfitInfos = new();
    [SerializeField] private List<UpgradePriceRow> upgradePriceRows = new();

    [Header("Spot Info")]
    [SerializeField] private Image imageSpot;
    [SerializeField] private TextMeshProUGUI textSpot;
    [SerializeField] private TextMeshProUGUI textLevel;
    [SerializeField] private TextMeshProUGUI textProfit;
    [SerializeField] private TextMeshProUGUI textNextCost;
    [SerializeField] private UIEffect effectUpgradeButton;
    [SerializeField] private Button buttonUpgrade;

    [Header("Progress Bar")]
    [SerializeField] private RectTransform progressBar; // твой индикатор
    [SerializeField] private float minWidth = 10f;
    [SerializeField] private float maxWidth = 780f;
    [SerializeField] private float tweenDuration = 0.3f; // время анимации

    public void Initialize()
    {
        for (int i = 0; i < casinoProfitCasinoTypes.Count; i++)
        {
            casinoProfitCasinoTypes[i].OnChooseProfitType += ChooseProfitType;
            casinoProfitCasinoTypes[i].Initialize();
        }

        effectUpgradeButton.Initialize();
        buttonUpgrade.onClick.AddListener(Upgrade);
    }

    public void Dispose()
    {
        for (int i = 0; i < casinoProfitCasinoTypes.Count; i++)
        {
            casinoProfitCasinoTypes[i].OnChooseProfitType -= ChooseProfitType;
            casinoProfitCasinoTypes[i].Dispose();
        }

        effectUpgradeButton.Dispose();
        buttonUpgrade.onClick.RemoveListener(Upgrade);
    }

    public void SetCasinoType(CasinoEntityType type)
    {
        var info = casinoProfitInfos.FirstOrDefault(data => data.CasinoEntityType == type);

        if(info == null)
        {
            Debug.LogWarning("Not found CasinoType - " + type);
            return;
        }

        imageSpot.sprite = info.Sprite;
        imageSpot.rectTransform.sizeDelta = info.VectorSize;
        textSpot.text = info.Name;
    }

    public void UpdateDetailPanel(CasinoEntityType type, int currentProfit, int level, int maxLevel, int nextCost)
    {
        SetCasinoType(type);

        textLevel.text = $"{level}/{maxLevel}";
        textProfit.text = currentProfit.ToString();
        textNextCost.text = nextCost > 0 ? $"{nextCost}$" : "Max";

        UpdateProgressBar(level, maxLevel);
        UpgradeStatusButton(level, maxLevel);

        var visual = upgradePriceRows.FirstOrDefault(data => data.Type == type);

        if(visual == null) return;

        visual.textPrice.text = nextCost > 0 ? $"{nextCost}$" : "Max";
    }

    public void UpdatePriceMain(CasinoEntityType type, int nextCost)
    {
        var visual = upgradePriceRows.FirstOrDefault(data => data.Type == type);

        if (visual == null) return;

        visual.textPrice.text = nextCost > 0 ? $"{nextCost}$" : "Max";
    }

    private void UpdateProgressBar(int currentLevel, int maxLevel)
    {
        float t = currentLevel / (float)maxLevel;
        float targetWidth = Mathf.Lerp(minWidth, maxWidth, t);

        progressBar.DOSizeDelta(
                new Vector2(targetWidth, progressBar.sizeDelta.y),
                tweenDuration
            );
    }

    private void UpgradeStatusButton(int level, int maxLevel)
    {
        if (level >= maxLevel)
        {
            buttonUpgrade.interactable = false;

            if(effectUpgradeButton.IsActive)
                effectUpgradeButton.DeactivateEffect();
        }
        else
        {
            buttonUpgrade.interactable = true;

            if (!effectUpgradeButton.IsActive)
                effectUpgradeButton.ActivateEffect();
        }
    }

    #region Output

    public event Action<CasinoEntityType> OnChooseProfitType;
    public event Action OnUpgrade;

    private void ChooseProfitType(CasinoEntityType entityType)
    {
        OnChooseProfitType(entityType);
    }

    private void Upgrade()
    {
        OnUpgrade?.Invoke();
    }

    #endregion

    [Serializable]
    private class CasinoProfitCasinoType
    {
        [SerializeField] private CasinoEntityType entityType;
        [SerializeField] private Button buttonType;

        public void Initialize()
        {
            buttonType.onClick.AddListener(ChooseProfitType);
        }

        public void Dispose()
        {
            buttonType.onClick.RemoveListener(ChooseProfitType);
        }

        #region Output

        public event Action<CasinoEntityType> OnChooseProfitType;

        private void ChooseProfitType()
        {
            OnChooseProfitType?.Invoke(entityType);
        }

        #endregion
    }

    [Serializable]
    private class CasinoProfitInfo
    {
        [SerializeField] private CasinoEntityType casinoEntityType;
        [SerializeField] private Sprite spriteSpot;
        [SerializeField] private string nameSpot;
        [SerializeField] private Vector2 vectorSize;

        public CasinoEntityType CasinoEntityType => casinoEntityType;
        public Sprite Sprite => spriteSpot;
        public string Name => nameSpot;
        public Vector2 VectorSize => vectorSize;
    }

    [Serializable]
    private class UpgradePriceRow
    {
        public CasinoEntityType Type;           // тип казино или столика
        public TextMeshProUGUI textPrice;      // текст для отображения цены/Max
    }
}