using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightPanel_Game : MovePanel
{
    [SerializeField] private UIEffectCombination effectCombination;
    [SerializeField] private Button buttonPause;
    [SerializeField] private Button buttonResults;

    public override void Initialize()
    {
        base.Initialize();

        effectCombination.Initialize();

        buttonPause.onClick.AddListener(ClickToPause);
        buttonResults.onClick.AddListener(ClickToResults);
    }

    public override void Dispose()
    {
        base.Dispose();

        effectCombination.Dispose();

        buttonPause.onClick.RemoveListener(ClickToPause);
        buttonResults.onClick.RemoveListener(ClickToResults);
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

    public event Action OnClickToPause;
    public event Action OnClickToResults;

    private void ClickToPause()
    {
        OnClickToPause?.Invoke();
    }

    private void ClickToResults()
    {
        OnClickToResults?.Invoke();
    }

    #endregion
}
