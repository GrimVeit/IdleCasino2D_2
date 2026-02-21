using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RotateEffect : UIEffect
{
    [Header("Settings")]
    public Transform target;
    public float rotationSpeed = 180f; // градусов в секунду
    public float duration = 2f;        // время вращения

    private Tween rotateTween;

    public override void Initialize()
    {

    }

    public override void Dispose()
    {

    }

    public override void ActivateEffect(Action OnComplete = null)
    {
        rotateTween?.Kill();

        rotateTween = target
            .DORotate(
                new Vector3(0f, 0f, -rotationSpeed * duration),
                duration,
                RotateMode.LocalAxisAdd
            )
            .SetEase(Ease.Linear);
    }
}
