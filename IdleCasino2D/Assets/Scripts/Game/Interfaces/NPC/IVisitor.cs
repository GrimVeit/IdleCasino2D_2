using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisitor : INpc
{
    //Target
    bool MoveNextStep();
    CasinoEntityType CurrentTarget { get; }

    //Emotions
    void ActivateWin();
    void ActivateLose();
    void ActivatePlay();
    void ActivateIdle();
    void Destroy();
}
