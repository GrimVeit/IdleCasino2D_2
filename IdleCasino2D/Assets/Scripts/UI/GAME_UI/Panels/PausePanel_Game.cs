using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel_Game : MovePanel
{
    [SerializeField] private UIEffectCombination effectCombination;
    [SerializeField] private Button buttonResume;

    public override void Initialize()
    {
        base.Initialize();

        effectCombination.Initialize();

        buttonResume.onClick.AddListener(ClickToResume);
    }

    public override void Dispose()
    {
        base.Dispose();

        effectCombination.Dispose();

        buttonResume.onClick.RemoveListener(ClickToResume);
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

    public event Action OnClickToResume;

    private void ClickToResume()
    {
        OnClickToResume?.Invoke();
    }

    #endregion
}
