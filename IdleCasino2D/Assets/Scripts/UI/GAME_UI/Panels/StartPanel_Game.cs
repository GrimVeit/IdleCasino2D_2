using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel_Game : MovePanel
{
    [SerializeField] private UIEffectCombination effectCombination;
    [SerializeField] private Button buttonPlay;
    [SerializeField] private SkeletonGraphic skeletonAnimationPlay;

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

        skeletonAnimationPlay.AnimationState.SetAnimation(0, "idle", loop: true);

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
        skeletonAnimationPlay.AnimationState.SetAnimation(0, "click", loop: false);

        OnClickToPlay?.Invoke();
    }

    #endregion
}
