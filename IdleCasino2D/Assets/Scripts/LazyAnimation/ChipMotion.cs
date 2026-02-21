using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.TestTools;

public class ChipMotion : MotionBase
{
    [SerializeField] private List<Transform> transformsPositions = new();
    [SerializeField] private Transform transformMotion;

    [SerializeField] private float rotateMinAngle;
    [SerializeField] private float rotateMaxAngle;
    [SerializeField] private float minDuration;
    [SerializeField] private float maxDuration;
    [SerializeField] private float moveRange;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private Tween _tweenMove;
    private Tween _tweenRotate;

    public override void Initialize()
    {
        _startPosition = transformMotion.localPosition;
        _startRotation = transformMotion.localRotation;
    }

    public override void Activate()
    {
        AnimateMove();
        AnimateRotate();
    }

    public override void Deactivate()
    {
        _tweenMove?.Kill();
        _tweenRotate?.Kill();

        transformMotion.SetLocalPositionAndRotation(_startPosition, _startRotation);
    }

    private void AnimateMove()
    {
        Vector3 target = transformsPositions[Random.Range(0, transformsPositions.Count)].localPosition + new Vector3(Random.Range(-moveRange, moveRange), Random.Range(-moveRange, moveRange), 0);
        float moveDuration = Random.Range(minDuration, maxDuration);
        _tweenMove = transformMotion
        .DOLocalMove(target, moveDuration);
    }
    private void AnimateRotate()
    {
        float rotateDuration = Random.Range(minDuration, maxDuration);

        float angle = Random.Range(rotateMinAngle, rotateMaxAngle);

        _tweenRotate = transformMotion
            .DOLocalRotate(new Vector3(0, 0, angle), rotateDuration)
            .SetEase(Ease.InOutSine);
    }
}
