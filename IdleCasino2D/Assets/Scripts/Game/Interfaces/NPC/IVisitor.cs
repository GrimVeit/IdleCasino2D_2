using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisitor : INpc
{
    //Interactive
    public event Action<IVisitor> OnClick;

    void Initialize();
    void Dispose();

    //Target
    bool HasNextStep();
    bool HasCurrentStep();
    void SetNextStep();
    CasinoEntityType CurrentTarget { get; }
    CasinoEntityType? SecondTarget { get; }

    //Emotions
    void ActivateWin();
    void ActivateLose();
    void ActivatePlay();
    void ActivateIdle();
    void Destroy();

    //Message
    void SetMessage(string message, SpeechTurnEnum turn);
    void SetMessage(string message);
}
