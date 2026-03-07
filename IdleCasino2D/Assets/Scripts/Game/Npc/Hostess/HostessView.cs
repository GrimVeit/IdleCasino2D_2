using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class HostessView : View, IStaffView
{
    [SerializeField] private ClickItem clickItem;
    public Vector3 Position => transform.position;
    public Node CurrentNode => _currentNode;

    [SerializeField] private float speedMove;
    [SerializeField] private HostessAnimations animations;

    private Node _currentNode;
    private int currentPointPath = 0;

    private Tween tweenMove;

    public void Initialize()
    {
        clickItem.OnClick += ClickItem;
    }

    public void Dispose()
    {
        clickItem.OnClick -= ClickItem;
    }

    public void Show()
    {
        transform.localScale = Vector3.zero;

        transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f)
            .SetEase(Ease.OutBack);
    }

    public void HideDestroy()
    {
        transform.DOScale(Vector3.zero, 0.25f)
        .SetEase(Ease.InBack)
        .OnComplete(() => Destroy(gameObject));
    }

    public void MoveTo(Node node, bool isAbsolute)
    {
        currentPointPath = 0;

        if (isAbsolute)
        {
            MovePath(new List<Node>() { node });
        }
        else
        {
            MovePath(Paths.FindPath(_currentNode, node));
        }
    }

    public void ActivateAnimation(HostessAnimationEnum visitorAnimation)
    {
        animations.ActivateAnimation(visitorAnimation);
    }

    public void SetOrder(int order)
    {
        animations.SetOrder(order);
    }

    public void SetSkin(string name)
    {
        animations.SetSkin(name);
    }

    public void SetMove(Node node)
    {
        tweenMove?.Kill();

        _currentNode = node;

        transform.localPosition = new Vector3(node.transform.localPosition.x, node.transform.localPosition.y, -5); ;
    }

    private void MovePath(List<Node> nodes)
    {
        tweenMove?.Kill();

        if (currentPointPath >= nodes.Count)
        {
            animations.ActivateAnimation(HostessAnimationEnum.Idle);
            OnPathCompleted?.Invoke(_currentNode);
            return;
        }

        animations.ActivateAnimation(HostessAnimationEnum.Walk);

        _currentNode = nodes[currentPointPath];

        Vector3 localTarget = new Vector3(_currentNode.transform.localPosition.x, _currentNode.transform.localPosition.y, -5);

        float distance = Vector3.Distance(transform.localPosition, localTarget);
        float duration = distance / speedMove;

        Vector3 direction = (localTarget - transform.localPosition).normalized;
        UpdateSkeletonDirection(direction);

        tweenMove = transform.DOLocalMove(localTarget, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                currentPointPath += 1;
                MovePath(nodes);
            });

    }

    public void Exit()
    {
        Destroy(gameObject);
    }

    private void UpdateSkeletonDirection(Vector3 dir)
    {
        if (dir.y >= 0)
        {
            if (dir.x < 0)
                animations.ActivateNpcRotation(NpcRotationEnum.BackLeft);
            else
                animations.ActivateNpcRotation(NpcRotationEnum.BackRight);
        }
        else
        {
            if (dir.x < 0)
                animations.ActivateNpcRotation(NpcRotationEnum.FrontLeft);
            else
                animations.ActivateNpcRotation(NpcRotationEnum.FrontRight);
        }
    }

    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum)
    {
        animations.ActivateNpcRotation(npcRotationEnum);
    }

    #region Output

    public event Action<Node> OnPathCompleted;
    public event Action OnClick;

    private void ClickItem()
    {
        OnClick?.Invoke();
    }

    #endregion
}

[Serializable]
public class HostessAnimations
{
    [SerializeField] private List<HostessAnimation> visitorAnimations = new List<HostessAnimation>();

    public void ActivateAnimation(HostessAnimationEnum animationEnum)
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
public class HostessAnimation
{
    public NpcRotationEnum NpcRotationEnum => npcRotationEnum;

    [SerializeField] private NpcRotationEnum npcRotationEnum;
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private List<HostessAnimationType> skeletonAnimationTypes = new List<HostessAnimationType>();

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

    public void ActivateAnimation(HostessAnimationEnum animationEnum)
    {
        var skeletonAnimationType = GetSkeletonAnimationType(animationEnum);
        if (skeletonAnimationType == null) return;

        skeletonAnimation.AnimationState.SetAnimation(0, skeletonAnimationType.Key, skeletonAnimationType.IsLoop);
    }

    private HostessAnimationType GetSkeletonAnimationType(HostessAnimationEnum animationEnum)
    {
        return skeletonAnimationTypes.Find(data => data.AnimationEnum == animationEnum);
    }
}

[Serializable]
public class HostessAnimationType
{
    [SerializeField] private string key;
    [SerializeField] private HostessAnimationEnum animationEnum;
    [SerializeField] private bool isLoop;

    public string Key => key;
    public HostessAnimationEnum AnimationEnum => animationEnum;
    public bool IsLoop => isLoop;
}
