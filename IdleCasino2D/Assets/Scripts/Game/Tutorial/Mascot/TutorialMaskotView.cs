using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TutorialMaskotView : View
{
    [SerializeField] private Transform transformMascot;

    [SerializeField] private Vector3 vectorShowRotation;
    [SerializeField] private Vector3 vectorHideRotation;
    [SerializeField] private float timeShowHide;

    private Tween tweenScale;
    private Tween tweenRot;

    public void Activate()
    {
        tweenScale?.Kill();
        tweenRot?.Kill();

        tweenScale = transformMascot.DOScale(1, timeShowHide);
        tweenRot = transformMascot.DOLocalRotate(vectorShowRotation, timeShowHide);
    }

    public void Deactivate()
    {
        tweenScale?.Kill();
        tweenRot?.Kill();

        tweenScale = transformMascot.DOScale(0, timeShowHide);
        tweenRot = transformMascot.DOLocalRotate(vectorHideRotation, timeShowHide);
    }
}
