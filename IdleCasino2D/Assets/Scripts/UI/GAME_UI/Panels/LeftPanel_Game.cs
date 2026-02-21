using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftPanel_Game : MovePanel
{
    [SerializeField] private UIEffectCombination effectCombination;
    [SerializeField] private Button buttonExit;

    public override void Initialize()
    {
        base.Initialize();

        effectCombination.Initialize();

        buttonExit.onClick.AddListener(ClickToExit);
    }

    public override void Dispose()
    {
        base.Dispose();

        effectCombination.Dispose();

        buttonExit.onClick.RemoveListener(ClickToExit);
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

    public event Action OnClickToExit;

    private void ClickToExit()
    {
        OnClickToExit?.Invoke();
    }

    #endregion
}
