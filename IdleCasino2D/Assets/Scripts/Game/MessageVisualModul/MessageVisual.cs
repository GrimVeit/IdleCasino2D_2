using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MessageVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMessage;

    private Tween tweenScale;

    public void SetText(string text)
    {
        textMessage.text = text;
    }

    public void Activate()
    {
        tweenScale?.Kill();

        tweenScale = transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
    }

    public void Deactivate()
    {
        tweenScale?.Kill();

        tweenScale = transform.DOScale(0, 0.2f).SetEase(Ease.InBack);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        tweenScale?.Kill();
    }

    #region Output

    public event Action<MessageVisual> OnDeactivate;

    #endregion
}
