using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICasinoEntityInfo
{
    CasinoEntityType CasinoEntityType { get; }
    bool CanJoin { get; }
    bool IsGameRunning {  get; }
}

public interface ICasinoEntityInteractiveProvider
{
    public void ActivateEntityInteractive();
    public void DeactivateEntityInteractive();
}

public interface ICasinoEntityVisitorTraffic
{
    void AddVisitor(IVisitor visitor);
    void RemoveVisitor(IVisitor visitor);

    event Action<IVisitor> OnVisitorRealised;
}

public interface ICasinoEntityProfit
{
    public event Action<Vector3, int> OnAddCoins;
}

public interface ICasinoEntityPersonal
{
    public void SetDealer(IDealer newDealer);
}
