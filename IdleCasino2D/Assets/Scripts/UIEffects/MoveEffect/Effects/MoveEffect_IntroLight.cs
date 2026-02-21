using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoveEffect_IntroLight : MoveEffect
{
    [SerializeField] private Transform moveElement;

    [Header("Points")]
    [SerializeField] private Transform transformLeft;
    [SerializeField] private Transform transformNormal;

    [Header("Time")]
    [SerializeField] private float timeMoveFromLeftToNormal;

    private Tween tweenMove;

    public override void Initialize()
    {

    }

    public override void Dispose()
    {
        tweenMove?.Kill();
    }

    public override void ActivateEffect(Action OnComplete = null)
    {
        tweenMove?.Kill();

        moveElement.transform.localPosition = transformLeft.localPosition;

        tweenMove = moveElement.DOLocalMove(transformNormal.localPosition, timeMoveFromLeftToNormal).SetEase(Ease.InCubic);
    }

    public override void DeactivateEffect(Action OnComplete = null)
    {
        tweenMove?.Kill();

        tweenMove = moveElement.DOLocalMove(transformLeft.localPosition, timeMoveFromLeftToNormal).SetEase(Ease.InCubic);
    }
}
