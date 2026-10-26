using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScreenPanel : MovePanel
{
    [SerializeField] private UIEffectCombination effectCombination;

    private void Awake() => Initialize();

    private void OnDestroy() => Dispose();


    public override void Initialize()
    {
        base.Initialize();

        effectCombination.Initialize();
    }

    public override void Dispose()
    {
        base.Dispose();

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
}
