using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationPanel_Menu : MovePanel
{
    [SerializeField] private Button buttonRegistrate;
    [SerializeField] private UIEffectCombination effectCombination;

    public override void Initialize()
    {
        base.Initialize();

        buttonRegistrate.onClick.AddListener(() => OnClickToRegistrate?.Invoke());

        effectCombination.Initialize();
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonRegistrate.onClick.RemoveListener(() => OnClickToRegistrate?.Invoke());

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

    public event Action OnClickToRegistrate;

    #endregion
}
