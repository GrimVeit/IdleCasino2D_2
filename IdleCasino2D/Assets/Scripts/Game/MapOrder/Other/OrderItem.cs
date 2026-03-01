using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OrderItem : MonoBehaviour
{
    public virtual Vector3 Position {  get; protected set; }

    public virtual void SetOrder(int order) { }
}
