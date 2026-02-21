using System;
using DG.Tweening;
using UnityEngine;

public class ScaleEffect_Fade : ScaleEffect
{
    [SerializeField] private float duration;
    [SerializeField] private Transform scaleElement;

    private Tween tweenFade;

    private Vector3 scaleNormal;

    public override void Initialize()
    {
        scaleNormal = scaleElement.localScale;
        scaleElement.localScale = Vector3.zero;
    }

    public override void Dispose()
    {
        tweenFade?.Kill();
    }

    public override void ResetEffect()
    {
        tweenFade?.Kill();

        isActive = false;

        scaleElement.localScale = Vector2.zero;
    }

    public override void ActivateEffect(Action OnComplete = null)
    {
        tweenFade?.Kill();

        isActive = true;

        tweenFade = scaleElement.DOScale(scaleNormal, duration).SetEase(Ease.OutBack);
    }

    public override void DeactivateEffect(Action OnComplete = null)
    {
        tweenFade?.Kill();

        isActive = false;

        tweenFade = scaleElement.DOScale(Vector3.zero, duration);
    }
}
