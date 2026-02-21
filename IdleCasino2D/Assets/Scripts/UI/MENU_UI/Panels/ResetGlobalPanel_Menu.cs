using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetGlobalPanel_Menu : MovePanel
{
    [SerializeField] private Button buttonBack;
    [SerializeField] private Button buttonReset;
    [SerializeField] private UIEffectCombination effectCombination;

    public override void Initialize()
    {
        base.Initialize();

        buttonBack.onClick.AddListener(() => OnClickToBack?.Invoke());
        buttonReset.onClick.AddListener(() => OnClickToReset?.Invoke());

        effectCombination.Initialize();
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonBack.onClick.RemoveListener(() => OnClickToBack?.Invoke());
        buttonReset.onClick.RemoveListener(() => OnClickToReset?.Invoke());

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
    public event Action OnClickToReset;

    #endregion
}
