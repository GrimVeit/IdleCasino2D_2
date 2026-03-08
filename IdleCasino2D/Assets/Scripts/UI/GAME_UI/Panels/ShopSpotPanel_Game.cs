using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class ShopSpotPanel_Game : MovePanel
{
    [SerializeField] private UIEffectCombination effectCombination;
    [SerializeField] private Button buttonBack;
    [SerializeField] private SkeletonGraphic skeletonGraphic_Star;

    public override void Initialize()
    {
        base.Initialize();

        effectCombination.Initialize();

        buttonBack.onClick.AddListener(ClickToBack);
    }

    public override void Dispose()
    {
        base.Dispose();

        effectCombination.Dispose();

        buttonBack.onClick.RemoveListener(ClickToBack);
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

    public event Action OnClickToBack;

    private void ClickToBack()
    {
        OnClickToBack?.Invoke();
    }

    #endregion
}
