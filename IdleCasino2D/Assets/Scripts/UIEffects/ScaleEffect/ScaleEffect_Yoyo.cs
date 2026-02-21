using System;
using DG.Tweening;
using UnityEngine;

public class ScaleEffect_Yoyo : ScaleEffect
{
    [SerializeField] private Vector3 scaleMax;
    [SerializeField] private Vector3 scaleMin;
    [SerializeField] private float duration;
    [SerializeField] private bool isAwake;
    [SerializeField] private Transform scaleElement;

    private Tween tweenYoyo;

    public override void Initialize()
    {
        scaleElement.localScale = scaleMin;

        if (isAwake)
            ActivateEffect();
    }

    public override void Dispose()
    {
        tweenYoyo?.Kill();
    }

    public override void ResetEffect()
    {
        tweenYoyo?.Kill();

        isActive = false;

        scaleElement.localScale = Vector2.zero;
    }

    public override void ActivateEffect(Action OnComplete = null)
    {
        tweenYoyo?.Kill();

        isActive = true;

        tweenYoyo = scaleElement.DOScale(scaleMax, duration).SetLoops(-1, LoopType.Yoyo);
    }

    public override void DeactivateEffect(Action OnComplete = null)
    {
        tweenYoyo?.Kill();

        isActive = false;

        tweenYoyo = scaleElement.DOScale(scaleMin, duration);
    }
}
