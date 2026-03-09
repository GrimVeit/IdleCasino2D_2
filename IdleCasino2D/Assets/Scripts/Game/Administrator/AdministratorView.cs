using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class AdministratorView : View
{
    [SerializeField] private SkeletonGraphic skeletonGraphicAdministrator;

    public void Activate()
    {
        skeletonGraphicAdministrator.AnimationState.SetAnimation(0, "score", true);
    }

    public void Deactivate()
    {
        skeletonGraphicAdministrator.AnimationState.SetAnimation(0, "idle", true);
    }
}
