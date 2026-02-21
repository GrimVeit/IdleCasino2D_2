using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationElement : MonoBehaviour
{
    public string Id => id;
    [SerializeField] private protected string id;

    public abstract void Activate(int cycles);
    public abstract void Deactivate();
}
