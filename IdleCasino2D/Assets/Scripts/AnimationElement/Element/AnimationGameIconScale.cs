using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnimationGameIconScale : AnimationElement
{
    [SerializeField] private Transform transformGameIcon;
    [SerializeField] private Vector3 vectorMinScale;
    [SerializeField] private Vector3 vectorMaxScale;
    [SerializeField] private float durationScale;

    private Sequence sequenceScale;

    public override void Activate(int cycles)
    {
        sequenceScale?.Kill();

        sequenceScale = DOTween.Sequence();

        sequenceScale
            .Append(transformGameIcon.DOScale(vectorMaxScale, durationScale))
            .Append(transformGameIcon.DOScale(vectorMinScale, durationScale));
    }

    public override void Deactivate()
    {
        sequenceScale?.Kill();
    }
}
