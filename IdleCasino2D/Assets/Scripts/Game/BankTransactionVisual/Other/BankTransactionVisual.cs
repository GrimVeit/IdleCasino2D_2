using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BankTransactionVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTransaction;

    private Tween tweenMove;
    private Tween tweenFade;

    public void SetColor(Color color)
    {
        textTransaction.color = color;
    }

    public void SetText(string text)
    {
        textTransaction.text = text;
    }

    public void Activate(Transform transformTargetLocal, float speed)
    {
        tweenMove?.Kill();
        tweenFade?.Kill();

        tweenMove = transform
            .DOLocalMove(transformTargetLocal.localPosition, speed)
            .SetEase(Ease.InCubic);

        tweenFade = textTransaction
            .DOFade(1, speed * 0.8f)
            .SetEase(Ease.OutQuad);
    }

    public void Deactivate(Transform transformTargetLocal, float speed)
    {
        tweenMove?.Kill();
        tweenFade?.Kill();

        tweenMove = transform
            .DOLocalMove(transformTargetLocal.localPosition, speed)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => OnDeactivate?.Invoke(this));

        tweenFade = textTransaction
            .DOFade(0, speed * 0.8f)
            .SetEase(Ease.InQuad);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        tweenMove?.Kill();
        tweenFade?.Kill();
    }

    #region Output

    public event Action<BankTransactionVisual> OnDeactivate;

    #endregion
}
