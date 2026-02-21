using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICasinoEntity
{
    CasinoEntityType CasinoEntityType { get; }

    int MaxSeats { get; }
    int OccupiedSeats { get; }
    bool HasFreeSeats { get; }
    bool CanJoin { get; }

    public void ActivateManualInteractive();
    public void DeactivateManualInteractive();

    void AddVisitor(IVisitor visitor);
    void RemoveVisitor(IVisitor visitor);

    public void SetDealer(IDealer newDealer);

    event Action<IVisitor, ICasinoEntity> OnVisitorRealised;
}
