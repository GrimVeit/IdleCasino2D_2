using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class AdministratorView : View
{
    [SerializeField] private SkeletonGraphic skeletonGraphicAdministrator;

    [Header("MOVE")]
    [SerializeField] private Transform transformMoney;
    [SerializeField] private Transform transformNickname;
    [SerializeField] private Transform transformStartMoney;
    [SerializeField] private Transform transformEndMoney;

    [SerializeField] private float time;

    private Tween tweenScaleNickname;
    private Tween tweenMoveMoney;

    public void Activate()
    {
        tweenMoveMoney?.Kill();
        tweenScaleNickname?.Kill();

        skeletonGraphicAdministrator.AnimationState.SetAnimation(0, "score", true);

        tweenMoveMoney = transformMoney.DOLocalMove(transformStartMoney.localPosition, time);
        tweenScaleNickname = transformNickname.DOScaleX(0, time);
    }

    public void Deactivate()
    {
        tweenMoveMoney?.Kill();
        tweenScaleNickname?.Kill();

        skeletonGraphicAdministrator.AnimationState.SetAnimation(0, "idle", true);

        tweenMoveMoney = transformMoney.DOLocalMove(transformEndMoney.localPosition, time);
        tweenScaleNickname = transformNickname.DOScaleX(1, time);
    }
}
