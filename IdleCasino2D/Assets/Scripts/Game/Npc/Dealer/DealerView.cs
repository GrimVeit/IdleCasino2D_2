using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class DealerView : View
{
    [SerializeField] private SkeletonAnimation animationSlot;

    public void Idle()
    {
        animationSlot.AnimationState.SetAnimation(0, "idle", true);
    }

    public void Play()
    {
        animationSlot.AnimationState.SetAnimation(0, "game", true);
    }
}
