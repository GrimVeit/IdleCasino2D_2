using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TestLoading : MonoBehaviour
{
    [SerializeField] private RectTransform[] dots;
    [SerializeField] private float spawnDuration = 0.2f;
    [SerializeField] private float pulseScale = 1.4f;
    [SerializeField] private float pulseDuration = 0.25f;

    private Sequence pulseSequence;
    private Sequence showSequence;
    private Sequence hideSequence;

    private void Awake()
    {
        ResetDots();
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        ResetDots();

        showSequence?.Kill();
        pulseSequence?.Kill();

        showSequence = DOTween.Sequence();

        for (int i = 0; i < dots.Length; i++)
        {
            RectTransform dot = dots[i];
            dot.localScale = Vector3.zero;
            showSequence.Append(dot.DOScale(1f, spawnDuration)
                .SetEase(Ease.OutBack));
        }

        showSequence.OnComplete(StartPulse);
    }

    private void StartPulse()
    {
        pulseSequence = DOTween.Sequence().SetLoops(-1);

        foreach (var dot in dots)
        {
            pulseSequence.Append(dot.DOScale(pulseScale, pulseDuration)
                .SetEase(Ease.OutSine));
            pulseSequence.Append(dot.DOScale(1f, pulseDuration)
                .SetEase(Ease.InSine));
        }
    }

    public void Deactivate()
    {
        pulseSequence?.Kill();
        showSequence?.Kill();

        hideSequence?.Kill();
        hideSequence = DOTween.Sequence();

        for (int i = 0; i < dots.Length; i++)
        {
            RectTransform dot = dots[i];
            hideSequence.Append(dot.DOScale(0f, spawnDuration)
                .SetEase(Ease.InBack));
        }

        hideSequence.OnComplete(() =>
        {
            ResetDots();
            gameObject.SetActive(false);
        });
    }

    private void ResetDots()
    {
        foreach (var dot in dots)
            dot.localScale = Vector3.zero;
    }
}
