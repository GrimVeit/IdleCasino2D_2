using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class OrderItem_SkeletonAnimation : OrderItem, ISortable
{
    [SerializeField] private SkeletonAnimation _spriteRenderer;

    public override Vector3 Position => transform.position;

    public override void SetOrder(int order)
    {
        _spriteRenderer.GetComponent<MeshRenderer>().sortingOrder = order;
    }
}
