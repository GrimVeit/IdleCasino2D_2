using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CasinoProfitView : View
{
    [SerializeField] private List<CasinoProfitCasinoType> casinoProfitCasinoTypes = new();
    [SerializeField] private List<CasinoProfitInfo> casinoProfitInfos = new();

    [Header("Spot Info")]
    [SerializeField] private Image imageSpot;
    [SerializeField] private TextMeshProUGUI textSpot;

    public void Initialize()
    {
        for (int i = 0; i < casinoProfitCasinoTypes.Count; i++)
        {
            casinoProfitCasinoTypes[i].OnChooseProfitType += ChooseProfitType;
            casinoProfitCasinoTypes[i].Initialize();
        }
    }

    public void Dispose()
    {
        for (int i = 0; i < casinoProfitCasinoTypes.Count; i++)
        {
            casinoProfitCasinoTypes[i].OnChooseProfitType -= ChooseProfitType;
            casinoProfitCasinoTypes[i].Dispose();
        }
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

    #region Output

    public event Action<CasinoEntityType> OnChooseProfitType;

    private void ChooseProfitType(CasinoEntityType entityType)
    {
        OnChooseProfitType(entityType);
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

}