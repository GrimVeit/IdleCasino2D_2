using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel_Game : MovePanel
{
    [SerializeField] private UIEffectCombination effectCombination;
    [SerializeField] private Button buttonBack;

    public override void Initialize()
    {
        base.Initialize();

        effectCombination.Initialize();

        buttonBack.onClick.AddListener(ClickToBack);
    }

    public override void Dispose()
    {
        base.Dispose();

        effectCombination.Dispose();

        buttonBack.onClick.RemoveListener(ClickToBack);
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

    private void ClickToBack()
    {
        OnClickToBack?.Invoke();
    }

    #endregion
}
