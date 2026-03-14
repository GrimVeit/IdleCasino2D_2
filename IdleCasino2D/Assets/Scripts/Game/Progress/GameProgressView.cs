using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameProgressView : View
{
    [SerializeField] private RectTransform bar;
    [SerializeField] private float maxWidth = 740f;
    [SerializeField] private float tweenDuration = 0.5f;

    private Tween tweenScale;

    public void SetProgress(int currentPoints, int maxPoints)
    {
        tweenScale?.Kill();

        float progress = (float)currentPoints / maxPoints;
        float targetWidth = progress * maxWidth;

        if(targetWidth >= maxWidth) targetWidth = maxWidth;

        tweenScale = bar.DOSizeDelta(new Vector2(targetWidth, bar.sizeDelta.y), tweenDuration)
           .SetEase(Ease.OutCubic);
    }
}
