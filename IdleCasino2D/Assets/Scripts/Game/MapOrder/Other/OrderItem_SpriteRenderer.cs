using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderItem_SpriteRenderer : OrderItem, ISortable
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public override Vector3 Position => transform.position;

    public override void SetOrder(int order)
    {
        _spriteRenderer.sortingOrder = order;
    }
}
