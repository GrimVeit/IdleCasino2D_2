using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGamePanel_Menu : MovePanel
{
    [SerializeField] private Button buttonBack;
    [SerializeField] private Button buttonPlay;
    [SerializeField] private UIEffectCombination combination;

    public override void Initialize()
    {
        base.Initialize();

        buttonBack.onClick.AddListener(ClickToBack);
        buttonPlay.onClick.AddListener(ClickToPlay);
        combination.Initialize();
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonBack.onClick.RemoveListener(ClickToBack);
        buttonPlay.onClick.RemoveListener(ClickToPlay);
        combination.Dispose();
    }

    public override void ActivatePanel()
    {
        base.ActivatePanel();

        combination.ActivateEffect();
    }

    public override void DeactivatePanel()
    {
        base.DeactivatePanel();

        combination.DeactivateEffect();
    }

    #region Output

    public event Action OnClickToBack;
    public event Action OnClickToPlay;

    private void ClickToBack()
    {
        OnClickToBack?.Invoke();
    }

    private void ClickToPlay()
    {
        OnClickToPlay?.Invoke();
    }

    #endregion
}
