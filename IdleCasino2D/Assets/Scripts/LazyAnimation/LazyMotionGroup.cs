using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LazyMotionGroup
{
    [SerializeField] private List<MotionBase> lazyMotions = new();

    public void Initialize()
    {
        lazyMotions.ForEach(m => m.Initialize());
    }

    public void Dispose()
    {
        lazyMotions.ForEach(m => m.Dispose());
    }

    public void Activate()
    {
        lazyMotions.ForEach(m => m.Activate());
    }

    public void Deactivate()
    {
        lazyMotions.ForEach(m => m.Deactivate());
    }
}
