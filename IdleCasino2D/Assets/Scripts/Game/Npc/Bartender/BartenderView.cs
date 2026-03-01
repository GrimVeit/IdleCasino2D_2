using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class BartenderView : View, IStaffView
{
    public Vector3 Position => transform.position;
    public Node CurrentNode => _currentNode;

    [SerializeField] private BartenderAnimations animations;

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

    public void ActivateAnimation(BartenderAnimationEnum visitorAnimation)
    {
        animations.ActivateAnimation(visitorAnimation);
    }

    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum)
    {
        animations.ActivateNpcRotation(npcRotationEnum);
    }
}

[Serializable]
public class BartenderAnimations
{
    [SerializeField] private List<BartenderAnimation> bartenderAnimations = new List<BartenderAnimation>();

    public void ActivateAnimation(BartenderAnimationEnum animationEnum)
    {
        bartenderAnimations.ForEach(data => data.ActivateAnimation(animationEnum));
    }

    public void SetOrder(int order)
    {
        bartenderAnimations.ForEach(data => data.SetOrder(order));
    }

    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum)
    {
        for (int i = 0; i < bartenderAnimations.Count; i++)
        {
            if (bartenderAnimations[i].NpcRotationEnum == npcRotationEnum)
                bartenderAnimations[i].Activate();
            else
                bartenderAnimations[i].Deactivate();
        }
    }
}

[Serializable]
public class BartenderAnimation
{
    public NpcRotationEnum NpcRotationEnum => npcRotationEnum;

    [SerializeField] private NpcRotationEnum npcRotationEnum;
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private List<SkeletonBartenderAnimationType> skeletonAnimationTypes = new List<SkeletonBartenderAnimationType>();

    public void SetOrder(int order)
    {
        skeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = order;
    }

    public void Activate()
    {
        skeletonAnimation.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        skeletonAnimation.gameObject.SetActive(false);
    }

    public void ActivateAnimation(BartenderAnimationEnum animationEnum)
    {
        var skeletonAnimationType = GetSkeletonAnimationType(animationEnum);
        if (skeletonAnimationType == null) return;

        skeletonAnimation.AnimationState.SetAnimation(0, skeletonAnimationType.Key, skeletonAnimationType.IsLoop);
    }

    private SkeletonBartenderAnimationType GetSkeletonAnimationType(BartenderAnimationEnum animationEnum)
    {
        return skeletonAnimationTypes.Find(data => data.AnimationEnum == animationEnum);
    }
}

[Serializable]
public class SkeletonBartenderAnimationType
{
    [SerializeField] private string key;
    [SerializeField] private BartenderAnimationEnum animationEnum;
    [SerializeField] private bool isLoop;

    public string Key => key;
    public BartenderAnimationEnum AnimationEnum => animationEnum;
    public bool IsLoop => isLoop;
}
