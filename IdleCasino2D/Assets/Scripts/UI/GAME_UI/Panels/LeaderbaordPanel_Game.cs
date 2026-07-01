using System;
using UnityEngine;
using UnityEngine.UI;

public class LeaderbaordPanel_Game : MovePanel
{
    [SerializeField] private UIEffectCombination combination;
    [SerializeField] private Button buttonExit;

    public override void Initialize()
    {
        base.Initialize();

        combination.Initialize();
        buttonExit.onClick.AddListener(ClickExit);
    }

    public override void Dispose()
    {
        base.Dispose();

        combination.Dispose();
        buttonExit.onClick.RemoveListener(ClickExit);
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

    public event Action OnClickToExit;

    private void ClickExit()
    {
        OnClickToExit?.Invoke();
    }

    #endregion
}
