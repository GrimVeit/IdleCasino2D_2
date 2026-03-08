using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class ChooseAvailableSpotPanel_Game : MovePanel
{
    [SerializeField] private UIEffectCombination effectCombination;
    [SerializeField] private Button buttonExit;
    [SerializeField] private SkeletonGraphic skeletonGraphic_Star;

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

        skeletonGraphic_Star.AnimationState.SetAnimation(0, "animation", false);

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
