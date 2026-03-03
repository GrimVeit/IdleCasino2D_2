using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HostessEntityView : View
{
    [SerializeField] private List<HostessTypeVisual> hostessTypeVisuals = new List<HostessTypeVisual>();

    [SerializeField] private Image imageSpot;
    [SerializeField] private TextMeshProUGUI textSpot;
    [SerializeField] private Button buttonLeave;

    public void Initialize()
    {
        buttonLeave.onClick.AddListener(Leave);
    }

    public void Dispose()
    {
        buttonLeave.onClick.RemoveListener(Leave);
    }

    public void SetCasinoEntityType(CasinoEntityType? casinoEntityType)
    {
        if(casinoEntityType == null) return;

        var visual = hostessTypeVisuals.FirstOrDefault(data => data.CasinoEntityType == casinoEntityType);

        imageSpot.rectTransform.sizeDelta = visual.VectorSize;
        imageSpot.sprite = visual.Sprite;
        textSpot.text = visual.Name;
    }

    #region Output

    public event Action OnLeave;

    private void Leave()
    {
        OnLeave?.Invoke();
    }

    #endregion
}

[System.Serializable]
public class HostessTypeVisual
{
    [SerializeField] private CasinoEntityType casinoEntityType;
    [SerializeField] private Sprite spriteSpot;
    [SerializeField] private string nameSpot;
    [SerializeField] private Vector2 vectorSize;

    public string Name => nameSpot;
    public Sprite Sprite => spriteSpot;
    public CasinoEntityType CasinoEntityType => casinoEntityType;
    public Vector2 VectorSize => vectorSize;
}
