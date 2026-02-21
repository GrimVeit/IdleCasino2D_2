using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishButtonsPanel_Game : MovePanel
{
    [SerializeField] private UIEffectCombination combination;
    [SerializeField] private Button buttonNewGame;
    [SerializeField] private Button buttonExit;

    public override void Initialize()
    {
        base.Initialize();

        buttonNewGame.onClick.AddListener(ClickNewGame);
        buttonExit.onClick.AddListener(ClickToExit);
        combination.Initialize();
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonNewGame.onClick.RemoveListener(ClickNewGame);
        buttonExit.onClick.RemoveListener(ClickToExit);
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

    public event Action OnClickToNewGame;
    public event Action OnClickToExit;

    private void ClickNewGame()
    {
        OnClickToNewGame?.Invoke();
    }

    private void ClickToExit()
    {
        OnClickToExit?.Invoke();
    }

    #endregion
}
