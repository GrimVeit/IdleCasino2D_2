using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class SongstressView : View, IStaffView
{
    public Vector3 Position => transform.position;
    public Node CurrentNode => _currentNode;

    [SerializeField] private SongstressAnimations animations;

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

    public void ActivateAnimation(SongstressAnimationEnum visitorAnimation)
    {
        animations.ActivateAnimation(visitorAnimation);
    }

    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum)
    {
        animations.ActivateNpcRotation(npcRotationEnum);
    }
}

[Serializable]
public class SongstressAnimations
{
    [SerializeField] private List<SongstressAnimation> songstressAnimations = new List<SongstressAnimation>();

    public void ActivateAnimation(SongstressAnimationEnum animationEnum)
    {
        songstressAnimations.ForEach(data => data.ActivateAnimation(animationEnum));
    }

    public void SetOrder(int order)
    {
        songstressAnimations.ForEach(data => data.SetOrder(order));
    }

    public void SetSkin(string name)
    {
        songstressAnimations.ForEach(data => data.SetSkin(name));
    }

    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum)
    {
        for (int i = 0; i < songstressAnimations.Count; i++)
        {
            if (songstressAnimations[i].NpcRotationEnum == npcRotationEnum)
                songstressAnimations[i].Activate();
            else
                songstressAnimations[i].Deactivate();
        }
    }
}

[Serializable]
public class SongstressAnimation
{
    public NpcRotationEnum NpcRotationEnum => npcRotationEnum;

    [SerializeField] private NpcRotationEnum npcRotationEnum;
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private List<SkeletonSongstressAnimationType> skeletonAnimationTypes = new List<SkeletonSongstressAnimationType>();

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

    public void ActivateAnimation(SongstressAnimationEnum animationEnum)
    {
        var skeletonAnimationType = GetSkeletonAnimationType(animationEnum);
        if (skeletonAnimationType == null) return;

        skeletonAnimation.AnimationState.SetAnimation(0, skeletonAnimationType.Key, skeletonAnimationType.IsLoop);
    }

    private SkeletonSongstressAnimationType GetSkeletonAnimationType(SongstressAnimationEnum animationEnum)
    {
        return skeletonAnimationTypes.Find(data => data.AnimationEnum == animationEnum);
    }
}

[Serializable]
public class SkeletonSongstressAnimationType
{
    [SerializeField] private string key;
    [SerializeField] private SongstressAnimationEnum animationEnum;
    [SerializeField] private bool isLoop;

    public string Key => key;
    public SongstressAnimationEnum AnimationEnum => animationEnum;
    public bool IsLoop => isLoop;
}
