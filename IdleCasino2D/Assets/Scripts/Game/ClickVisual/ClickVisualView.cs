using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ClickVisualView : View
{
    [SerializeField] private List<ClickPoint> clickPoints = new List<ClickPoint>();

    [Header("References")]
    [SerializeField] private Transform root;     // Родитель всего hint-а
    [SerializeField] private Transform finger;   // Картинка пальца


    [Header("Show / Hide")]
    [SerializeField] private float showDuration = 0.25f;
    [SerializeField] private float hideDuration = 0.2f;

    [Header("Movement")]
    [SerializeField] private float moveDuration = 0.35f;

    [Header("Finger Yoyo")]
    [SerializeField] private float yoyoMinScale = 0.7f;
    [SerializeField] private float yoyoMaxScale = 1.1f;
    [SerializeField] private float yoyoDuration = 0.5f;

    private Sequence fingerSequence;

    private Tween tweenRootScale;
    private Tween tweenRootMove;

    private readonly Dictionary<string, Transform> transformPoints = new();


    public void Initialize()
    {
        root.localScale = Vector3.zero;
        finger.localScale = Vector3.one;

        transformPoints.Clear();

        for (int i = 0; i < clickPoints.Count; i++)
        {
            transformPoints.Add(
                clickPoints[i].Id,
                clickPoints[i].TransformPoint
            );
        }
    }


    public void Dispose()
    {
        root.DOKill();
        StopFingerAnimation();

        transformPoints.Clear();
    }


    public void Show()
    {
        gameObject.SetActive(true);

        tweenRootScale?.Kill();

        StopFingerAnimation();

        root.localScale = Vector3.zero;

        PlayFingerAnimation();

        tweenRootScale = root
            .DOScale(1f, showDuration)
            .SetEase(Ease.OutBack);
    }


    public void Hide()
    {
        tweenRootScale?.Kill();

        StopFingerAnimation();

        tweenRootScale = root
            .DOScale(Vector3.zero, hideDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }


    public void MoveTo(Vector3 position)
    {
        tweenRootMove?.Kill();

        tweenRootMove = root.DOLocalMove(position, moveDuration)
            .SetEase(Ease.OutQuad);
    }


    public void MoveTo(string id)
    {
        if (transformPoints.TryGetValue(id, out Transform point))
        {
            MoveTo(point.localPosition);
        }
    }


    public void SetPosition(Vector3 position)
    {
        tweenRootMove?.Kill();

        root.localPosition = position;
    }


    private void PlayFingerAnimation()
    {
        StopFingerAnimation();

        finger.localScale = Vector3.one * yoyoMinScale;

        fingerSequence = DOTween.Sequence();

        fingerSequence.Append(
            finger.DOScale(
                Vector3.one * yoyoMaxScale,
                yoyoDuration
            )
            .SetEase(Ease.InOutSine)
        );

        fingerSequence.Append(
            finger.DOScale(
                Vector3.one * yoyoMinScale,
                yoyoDuration
            )
            .SetEase(Ease.InOutSine)
        );

        fingerSequence.SetLoops(-1);
    }


    private void StopFingerAnimation()
    {
        if (fingerSequence != null)
        {
            fingerSequence.Kill();
            fingerSequence = null;
        }

        finger.DOKill();

        finger.localScale = Vector3.one;
    }

    [Serializable]
    private class ClickPoint
    {
        public string Id => id;
        public Transform TransformPoint => transformPoint;

        [SerializeField] private string id;
        [SerializeField] private Transform transformPoint;
    }
}
