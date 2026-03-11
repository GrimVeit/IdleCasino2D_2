using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class ManagerView : View, IStaffView
{
    public Vector3 Position => transform.position;
    public Node CurrentNode => _currentNode;

    [SerializeField] private ManagerAnimations animations;

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

    public void ActivateAnimation(ManagerAnimationEnum visitorAnimation)
    {
        animations.ActivateAnimation(visitorAnimation);
    }

    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum)
    {
        animations.ActivateNpcRotation(npcRotationEnum);
    }
}

[Serializable]
public class ManagerAnimations
{
    [SerializeField] private List<ManagerAnimation> managerAnimations = new List<ManagerAnimation>();

    public void ActivateAnimation(ManagerAnimationEnum animationEnum)
    {
        managerAnimations.ForEach(data => data.ActivateAnimation(animationEnum));
    }

    public void SetOrder(int order)
    {
        managerAnimations.ForEach(data => data.SetOrder(order));
    }

    public void SetSkin(string name)
    {
        managerAnimations.ForEach(data => data.SetSkin(name));
    }

    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum)
    {
        for (int i = 0; i < managerAnimations.Count; i++)
        {
            if (managerAnimations[i].NpcRotationEnum == npcRotationEnum)
                managerAnimations[i].Activate();
            else
                managerAnimations[i].Deactivate();
        }
    }
}

[Serializable]
public class ManagerAnimation
{
    public NpcRotationEnum NpcRotationEnum => npcRotationEnum;

    [SerializeField] private NpcRotationEnum npcRotationEnum;
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private List<SkeletonManagerAnimationType> skeletonAnimationTypes = new List<SkeletonManagerAnimationType>();

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

    public void ActivateAnimation(ManagerAnimationEnum animationEnum)
    {
        var skeletonAnimationType = GetSkeletonAnimationType(animationEnum);
        if (skeletonAnimationType == null) return;

        skeletonAnimation.AnimationState.SetAnimation(0, skeletonAnimationType.Key, skeletonAnimationType.IsLoop);
    }

    private SkeletonManagerAnimationType GetSkeletonAnimationType(ManagerAnimationEnum animationEnum)
    {
        return skeletonAnimationTypes.Find(data => data.AnimationEnum == animationEnum);
    }
}

[Serializable]
public class SkeletonManagerAnimationType
{
    [SerializeField] private string key;
    [SerializeField] private ManagerAnimationEnum animationEnum;
    [SerializeField] private bool isLoop;

    public string Key => key;
    public ManagerAnimationEnum AnimationEnum => animationEnum;
    public bool IsLoop => isLoop;
}
