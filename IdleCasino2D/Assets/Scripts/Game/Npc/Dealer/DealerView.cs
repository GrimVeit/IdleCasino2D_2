using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class DealerView : View, IStaffView
{
    public Vector3 Position => transform.position;
    public Node CurrentNode => _currentNode;

    [SerializeField] private DealerAnimations animations;

    private Node _currentNode;

    public void Show()
    {
        transform.localScale = Vector3.zero;

        transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.25f)
            .SetEase(Ease.OutBack);
    }

    public void HideDestroy()
    {
        transform.DOScale(Vector3.zero, 0.25f)
        .SetEase(Ease.InBack)
        .OnComplete(() => Destroy(gameObject));
    }

    public void SetMove(Node node)
    {
        _currentNode = node;

        transform.localPosition = _currentNode.transform.localPosition;
    }

    public void SetOrder(int order)
    {
        animations.SetOrder(order);
    }

    public void SetSkin(string name)
    {
        animations.SetSkin(name);
    }

    public void ActivateAnimation(DealerAnimationEnum visitorAnimation)
    {
        animations.ActivateAnimation(visitorAnimation);
    }

    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum)
    {
        animations.ActivateNpcRotation(npcRotationEnum);
    }
}

[Serializable]
public class DealerAnimations
{
    [SerializeField] private List<DealerAnimation> visitorAnimations = new List<DealerAnimation>();

    public void ActivateAnimation(DealerAnimationEnum animationEnum)
    {
        visitorAnimations.ForEach(data => data.ActivateAnimation(animationEnum));
    }

    public void SetOrder(int order)
    {
        visitorAnimations.ForEach(data => data.SetOrder(order));
    }

    public void SetSkin(string name)
    {
        visitorAnimations.ForEach(data => data.SetSkin(name));
    }

    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum)
    {
        for (int i = 0; i < visitorAnimations.Count; i++)
        {
            if (visitorAnimations[i].NpcRotationEnum == npcRotationEnum)
                visitorAnimations[i].Activate();
            else
                visitorAnimations[i].Deactivate();
        }
    }
}

[Serializable]
public class DealerAnimation
{
    public NpcRotationEnum NpcRotationEnum => npcRotationEnum;

    [SerializeField] private NpcRotationEnum npcRotationEnum;
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private List<SkeletonDealerAnimationType> skeletonAnimationTypes = new List<SkeletonDealerAnimationType>();

    public void SetOrder(int order)
    {
        skeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = order;
    }

    public void SetSkin(string name)
    {
        skeletonAnimation.Skeleton.SetSkin(name);
    }

    public void Activate()
    {
        skeletonAnimation.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        skeletonAnimation.gameObject.SetActive(false);
    }

    public void ActivateAnimation(DealerAnimationEnum animationEnum)
    {
        var skeletonAnimationType = GetSkeletonAnimationType(animationEnum);
        if (skeletonAnimationType == null) return;

        skeletonAnimation.AnimationState.SetAnimation(0, skeletonAnimationType.Key, skeletonAnimationType.IsLoop);
    }

    private SkeletonDealerAnimationType GetSkeletonAnimationType(DealerAnimationEnum animationEnum)
    {
        return skeletonAnimationTypes.Find(data => data.AnimationEnum == animationEnum);
    }
}

[Serializable]
public class SkeletonDealerAnimationType
{
    [SerializeField] private string key;
    [SerializeField] private DealerAnimationEnum animationEnum;
    [SerializeField] private bool isLoop;

    public string Key => key;
    public DealerAnimationEnum AnimationEnum => animationEnum;
    public bool IsLoop => isLoop;
}
