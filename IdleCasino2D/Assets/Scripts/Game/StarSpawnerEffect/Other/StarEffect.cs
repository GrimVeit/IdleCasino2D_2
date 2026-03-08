using System;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class StarEffect : MonoBehaviour
{
    public bool IsWork => isWork;

    public SkeletonAnimation skeletonAnimation;
    public event Action<StarEffect> OnEndAnimation;
    private bool isWork = false;

    public void Activate()
    {
        isWork = true;

        float duration = UnityEngine.Random.Range(5f, 10f);

        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 0.5f).SetEase(Ease.OutQuad);

        skeletonAnimation.AnimationState.TimeScale = UnityEngine.Random.Range(0.2f, 1f);
        skeletonAnimation.AnimationState.SetAnimation(0, "animation", false);

        DOVirtual.DelayedCall(duration, () =>
        {
            transform.DOScale(0f, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                isWork = false;
            });
        });
    }
}
