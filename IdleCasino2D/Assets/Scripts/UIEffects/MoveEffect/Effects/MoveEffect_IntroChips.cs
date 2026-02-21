using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoveEffect_IntroChips : MoveEffect
{
    [SerializeField] private Transform moveElement;

    [Header("Points")]
    [SerializeField] private Transform transformUp;
    [SerializeField] private Transform transformDown;
    [SerializeField] private Transform transformSmallUp;
    [SerializeField] private Transform transformNormal;

    [Header("Time")]
    [SerializeField] private float timeMoveFromUpToDown;
    [SerializeField] private float timeMoveFromDownToSmallUp;
    [SerializeField] private float timeMoveFromSmallUpToNormal;

    private Sequence seq;

    public override void Initialize()
    {

    }

    public override void Dispose()
    {
        seq?.Kill();
    }

    public override void ActivateEffect(Action OnComplete = null)
    {
        seq?.Kill();

        moveElement.transform.localPosition = transformUp.localPosition;

        seq = DOTween.Sequence();

        //Down
        seq.Append(moveElement.DOLocalMove(transformDown.localPosition, timeMoveFromUpToDown).SetEase(Ease.InCubic));

        //Small Up
        seq.Append(moveElement.DOLocalMove(transformSmallUp.localPosition, timeMoveFromDownToSmallUp).SetEase(Ease.OutCubic));

        //Normal
        seq.Append(moveElement.DOLocalMove(transformNormal.localPosition, timeMoveFromSmallUpToNormal).SetEase(Ease.InCubic));
    }

    public override void DeactivateEffect(Action OnComplete = null)
    {
        seq?.Kill();

        seq = DOTween.Sequence();

        //Small Up
        seq.Append(moveElement.DOLocalMove(transformSmallUp.localPosition, timeMoveFromSmallUpToNormal).SetEase(Ease.InCubic));

        //Down
        seq.Append(moveElement.DOLocalMove(transformDown.localPosition, timeMoveFromUpToDown).SetEase(Ease.OutCubic));

        //Down
        seq.Append(moveElement.DOLocalMove(transformUp.localPosition, timeMoveFromUpToDown).SetEase(Ease.InCubic));
    }
}
