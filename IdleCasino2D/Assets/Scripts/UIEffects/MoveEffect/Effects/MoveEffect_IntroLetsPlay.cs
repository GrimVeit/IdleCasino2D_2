using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoveEffect_IntroLetsPlay : MoveEffect
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

    [Header("Stars")]
    [SerializeField] private GameObject star_1;
    [SerializeField] private GameObject star_2;
    [SerializeField] private GameObject star_3;

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

        star_1.SetActive(false);
        star_2.SetActive(false);
        star_3.SetActive(false);

        seq = DOTween.Sequence();

        //Down
        seq.Append(moveElement.DOLocalMove(transformDown.localPosition, timeMoveFromUpToDown).SetEase(Ease.InCubic).OnComplete(() => { star_1.SetActive(true); }));

        //Small Up
        seq.Append(moveElement.DOLocalMove(transformSmallUp.localPosition, timeMoveFromDownToSmallUp).SetEase(Ease.OutCubic).OnComplete(() => { star_2.SetActive(true); }));

        //Normal
        seq.Append(moveElement.DOLocalMove(transformNormal.localPosition, timeMoveFromSmallUpToNormal).SetEase(Ease.InCubic).OnComplete(() => { star_3.SetActive(true); }));
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
        seq.Append(moveElement.DOLocalMove(transformUp.localPosition, timeMoveFromUpToDown).SetEase(Ease.InCubic)).OnComplete(() =>
        {
            star_1.SetActive(false);
            star_2.SetActive(false);
            star_3.SetActive(false);
        });
    }
}
