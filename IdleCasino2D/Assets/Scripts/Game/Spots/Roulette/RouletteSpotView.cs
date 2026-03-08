using System;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class RouletteSpotView : View, IIdentify
{
    public string GetID() => id;
    [SerializeField] private string id;
    [SerializeField] private ClickItem itemRouletteTable;
    [SerializeField] private SkeletonAnimation animationSlot;
    [SerializeField] private Transform transformStar;

    private Tween tweenHighlight;

    public void Initialize()
    {
        itemRouletteTable.OnClick += Click;
    }

    public void Dispose()
    {
        itemRouletteTable.OnClick -= Click;
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
        animationSlot.AnimationState.SetAnimation(0, "idle", true);
    }

    private void Close()
    {
        animationSlot.AnimationState.SetAnimation(0, "not open", true);
    }

    private void Play()
    {
        animationSlot.AnimationState.SetAnimation(0, "game", false);
    }

    #region Output

    public event Action OnClick;

    private void Click()
    {
        OnClick?.Invoke();
    }

    #endregion
}
