using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorModel
{
    public CasinoEntityType CurrentTarget => routeTargets[currentStepTarget];

    private readonly List<CasinoEntityType> routeTargets;

    public VisitorModel(List<CasinoEntityType> routeTargets)
    {
        this.routeTargets = routeTargets;
    }

    private int currentStepTarget = 0;

    public bool MoveNextStep()
    {
        currentStepTarget++;
        return currentStepTarget <= routeTargets.Count;
    }

    public void MoveTo(Node target, bool isAbsolute)
    {
        OnStartMove?.Invoke(target, isAbsolute);
    }

    #region Output

    public event Action<Node, bool> OnStartMove;

    #endregion
}
