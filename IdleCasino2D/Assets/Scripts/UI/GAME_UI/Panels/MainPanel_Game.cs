using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel_Game : MovePanel
{
    [SerializeField] private UIEffectCombination effectCombination;
    [SerializeField] private Button buttonUpgrade;
    [SerializeField] private Button buttonHireStaff;

    public override void Initialize()
    {
        base.Initialize();

        effectCombination.Initialize();

        buttonUpgrade.onClick.AddListener(ClickToUpgrade);
        buttonHireStaff.onClick.AddListener(ClickToHireStaff);
    }

    public override void Dispose()
    {
        base.Dispose();

        effectCombination.Dispose();

        buttonUpgrade.onClick.RemoveListener(ClickToUpgrade);
        buttonHireStaff.onClick.RemoveListener(ClickToHireStaff);
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

    public event Action OnClickToUpgrade;
    public event Action OnClickToHireStaff;

    private void ClickToUpgrade()
    {
        OnClickToUpgrade?.Invoke();
    }

    private void ClickToHireStaff()
    {
        OnClickToHireStaff?.Invoke();
    }

    #endregion
}
