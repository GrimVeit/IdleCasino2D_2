using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel_Game : MovePanel
{
    [SerializeField] private UIEffectCombination effectCombination;
    [SerializeField] private Button buttonPlay;

    public override void Initialize()
    {
        base.Initialize();

        effectCombination.Initialize();

        buttonPlay.onClick.AddListener(ClickToPlay);
    }

    public override void Dispose()
    {
        base.Dispose();

        effectCombination.Dispose();

        buttonPlay.onClick.RemoveListener(ClickToPlay);
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

    public event Action OnClickToPlay;

    private void ClickToPlay()
    {
        OnClickToPlay?.Invoke();
    }

    #endregion
}
