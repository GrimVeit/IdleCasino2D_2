using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class WheelSpotView : View, IIdentify
{
    public string GetID() => id;
    [SerializeField] private string id;
    [SerializeField] private ClickItem itemPokerTable;
    [SerializeField] private SkeletonAnimation animationWheel;
    [SerializeField] private Transform transformStar;

    private Tween tweenHighlight;

    public void Initialize()
    {
        itemPokerTable.OnClick += Click;
    }

    public void Dispose()
    {
        itemPokerTable.OnClick -= Click;
    }

    public void ActivateHighlight()
    {
        tweenHighlight?.Kill();

        tweenHighlight = transformStar.DOScale(1, 0.2f);
    }

    public void DeactivateHighlight()
    {
        tweenHighlight?.Kill();

        tweenHighlight = transformStar.DOScale(0, 0.2f);
    }

    public void SetAnimation(string name)
    {
        Debug.Log(name);

        switch (name)
        {
            case "idle":
                Idle();
                break;
            case "game":
                Play();
                break;
            case "not open":
                Close();
                break;
            default:
                Debug.LogWarning($"Not found Animation with id - {name}");
                break;
        }
    }

    private void Idle()
    {
        animationWheel.AnimationState.SetAnimation(0, "idle", true);
    }

    private void Close()
    {
        animationWheel.AnimationState.SetAnimation(0, "not open", true);
    }

    private void Play()
    {
        animationWheel.AnimationState.SetAnimation(0, "game", false);
    }

    #region Output

    public event Action OnClick;

    private void Click()
    {
        OnClick?.Invoke();
    }

    #endregion
}
