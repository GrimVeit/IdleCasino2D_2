using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class TestOrderItem : MonoBehaviour, ISortable
{
    [SerializeField] private SkeletonAnimation _spriteRenderer;

    public Vector3 Position => transform.position;

    public void SetOrder(int order)
    {
        _spriteRenderer.GetComponent<MeshRenderer>().sortingOrder = order;
    }
}
