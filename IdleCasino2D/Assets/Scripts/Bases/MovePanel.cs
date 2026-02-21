using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePanel : Panel
{
    public bool IsActive => isActive;

    [SerializeField] protected Vector3 from;
    [SerializeField] protected Vector3 to;
    [SerializeField] protected float time;
    [SerializeField] protected CanvasGroup canvasGroup;
    protected Tween tween;

    private bool isActive = false;

    public override void ActivatePanel()
    {
        if (tween != null) { tween?.Kill(); }

        panel.SetActive(true);
        isActive = true;
        tween = panel.transform.DOLocalMove(to, time).OnComplete(() =>
        {
            OnActivatePanel?.Invoke();
            OnActivatePanel_Data?.Invoke(this);
        });
        CanvasGroupAlpha(canvasGroup, 0, 1, time);
    }

    public override void DeactivatePanel()
    {
        if (tween != null) { tween?.Kill(); }

        isActive = false;
        tween = panel.transform.DOLocalMove(from, time).OnComplete(() => 
        {
            panel.SetActive(false);
            OnDeactivatePanel?.Invoke();
            OnDeactivatePanel_Data?.Invoke(this);
        });
        CanvasGroupAlpha(canvasGroup, 1, 0, time);
    }

    private void CanvasGroupAlpha(CanvasGroup canvasGroup, float from, float to, float time)
    {
        if(canvasGroup == null) return;

        Coroutines.Start(SmoothVal(canvasGroup, from, to, time));
    }

    private IEnumerator SmoothVal(CanvasGroup canvasGroup, float from, float to, float timer)
    {
        float t = 0.0f;
        canvasGroup.alpha = from;

        while (t < 1.0f)
        {
            t += Time.deltaTime * (1.0f / timer);
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(from, to, t);
            yield return 0;
        }
    }

    #region Input

    public event Action<MovePanel> OnDeactivatePanel_Data;
    public event Action<MovePanel> OnActivatePanel_Data;

    public event Action OnDeactivatePanel;
    public event Action OnActivatePanel;

    #endregion
}
