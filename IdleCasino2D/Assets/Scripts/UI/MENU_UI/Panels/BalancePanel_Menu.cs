using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalancePanel_Menu : MovePanel
{
    [SerializeField] private Button buttonBack;
    [SerializeField] private Button buttonShop;
    [SerializeField] private Button buttonPlay;
    [SerializeField] private UIEffectCombination effectCombination;

    public override void Initialize()
    {
        base.Initialize();

        buttonBack.onClick.AddListener(() => OnClickToBack?.Invoke());
        buttonShop.onClick.AddListener(() => OnClickToShop?.Invoke());
        buttonPlay.onClick.AddListener(() => OnClickToPlay?.Invoke());

        effectCombination.Initialize();
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonBack.onClick.RemoveListener(() => OnClickToBack?.Invoke());
        buttonShop.onClick.RemoveListener(() => OnClickToShop?.Invoke());
        buttonPlay.onClick.RemoveListener(() => OnClickToPlay?.Invoke());

        effectCombination.Dispose();
    }

    public override void ActivatePanel()
    {
        base.ActivatePanel();

        effectCombination.ActivateEffect();
    }

    public override void DeactivatePanel()
    {
        base.DeactivatePanel();

        effectCombination.DeactivateEffect();
    }

    #region Output

    public event Action OnClickToBack;
    public event Action OnClickToShop;
    public event Action OnClickToPlay;

    #endregion
}
