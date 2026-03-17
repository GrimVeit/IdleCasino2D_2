using System;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class BartenderView : View, IStaffView
{
    [SerializeField] private ClickItem clickItem;
    [SerializeField] private Canvas canvas;
    [SerializeField] private MessageVisualModul messageVisualModul;
    public Vector3 Position => transform.position;
    public Node CurrentNode => _currentNode;

    [SerializeField] private float speedMove;
    [SerializeField] private BartenderAnimations animations;

    private Node _currentNode;

    private Tween tweenMove;

    public void Initialize()
    {
        clickItem.OnClick += ClickItem;
    }

    public void Dispose()
    {
        clickItem.OnClick -= ClickItem;
    }

    public void SetMessage(string message, SpeechTurnEnum turnEnum)
    {
        messageVisualModul.SetMessage(message, turnEnum);
    }

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

    public void MoveTo(Node node, bool isAbsolute)
    {
        if (isAbsolute)
        {
            MovePath(node);
        }
        else
        {
            //MovePath(Paths.FindPath(_currentNode, node));
        }
    }

    private void MovePath(Node node)
    {
        tweenMove?.Kill();

        _currentNode = node;

        Vector3 localTarget = new Vector3(_currentNode.transform.localPosition.x, _currentNode.transform.localPosition.y, -5);

        float distance = Vector3.Distance(transform.localPosition, localTarget);
        float duration = distance / speedMove;

        tweenMove = transform.DOLocalMove(localTarget, duration)
            .SetEase(Ease.Linear);

    }

    public void SetOrder(int order)
    {
        animations.SetOrder(order);

        canvas.sortingOrder = order;
    }

    public void SetSkin(string name)
    {
        animations.SetSkin(name);
    }

    public void ActivateAnimation(BartenderAnimationEnum visitorAnimation)
    {
        animations.ActivateAnimation(visitorAnimation);
    }

    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum)
    {
        animations.ActivateNpcRotation(npcRotationEnum);
    }

    #region Output

    public event Action OnClick;

    private void ClickItem()
    {
        OnClick?.Invoke();
    }

    #endregion
}

[Serializable]
public class BartenderAnimations
{
    public NpcRotationEnum CurrentRotationEnum = NpcRotationEnum.None;

    [SerializeField] private List<BartenderAnimation> bartenderAnimations = new List<BartenderAnimation>();

    public void ActivateAnimation(BartenderAnimationEnum animationEnum)
    {
        bartenderAnimations.ForEach(data => data.ActivateAnimation(animationEnum));
    }

    public void SetOrder(int order)
    {
        bartenderAnimations.ForEach(data => data.SetOrder(order));
    }

    public void SetSkin(string name)
    {
        bartenderAnimations.ForEach(data => data.SetSKin(name));
    }

    public void ActivateNpcRotation(NpcRotationEnum npcRotationEnum)
    {
        for (int i = 0; i < bartenderAnimations.Count; i++)
        {
            if (bartenderAnimations[i].NpcRotationEnum == npcRotationEnum )
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

    public void SetSKin(string name)
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
