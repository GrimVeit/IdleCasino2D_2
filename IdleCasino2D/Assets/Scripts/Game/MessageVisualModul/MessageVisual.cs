using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMessage;
    [SerializeField] private Image imageBackground;

    private Tween tweenScale;

    public void SetColorText(Color color)
    {
        textMessage.color = color;
    }

    public void SetColorBackground(Color color)
    {
        imageBackground.color = color;
    }

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
