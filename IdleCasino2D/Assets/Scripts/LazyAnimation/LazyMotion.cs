using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LazyMotion : MotionBase
{
    [SerializeField] private Transform transformLazyMotion;

    [Header("Move")]
    [SerializeField] private float moveRange;
    [SerializeField] private float moveMinDuration;
    [SerializeField] private float moveMaxDuration;

    [Header("Rotate")]
    [SerializeField] private float rotateMinAngle;
    [SerializeField] private float rotateMaxAngle;
    [SerializeField] private bool isInverseDirection;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private Tween _moveTween;
    private Tween _rotateTween;


    public override void Initialize()
    {
        _startPosition = transformLazyMotion.localPosition;
        _startRotation = transformLazyMotion.localRotation;
    }

    public override void Activate()
    {
        AnimateMove();
        AnimateRotate();
    }

    public override void Deactivate()
    {
        _moveTween?.Kill();
        _rotateTween?.Kill();

        transformLazyMotion.SetLocalPositionAndRotation(_startPosition, _startRotation);
    }

    private void AnimateMove()
    {
        Vector3 target = _startPosition + new Vector3(Random.Range(-moveRange, moveRange), Random.Range(-moveRange, moveRange), 0);

        float moveDuration = Random.Range(moveMinDuration, moveMaxDuration);

        _moveTween = transformLazyMotion
            .DOLocalMove(target, moveDuration);
    }

    private void AnimateRotate()
    {
        float rotateDuration = Random.Range(moveMinDuration, moveMaxDuration);

        float angle = Random.Range(rotateMinAngle, rotateMaxAngle);

        if (isInverseDirection)
        {
            if (Random.value > 0.5f)
                angle = -angle;
        }

        _rotateTween = transformLazyMotion
            .DOLocalRotate(new Vector3(0, 0, angle), rotateDuration)
            .SetEase(Ease.InOutSine);
    }
}
