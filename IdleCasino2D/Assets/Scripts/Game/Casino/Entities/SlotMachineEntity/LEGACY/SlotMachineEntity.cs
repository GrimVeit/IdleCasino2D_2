using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachineEntity : MonoBehaviour
{
    public event Action<IVisitor, ICasinoEntity> OnVisitorRealised;
    public int MaxSeats => 1;
    public int OccupiedSeats => visitors.Count;
    public bool HasFreeSeats => visitors.Count < 1;
    public bool CanJoin => HasFreeSeats;

    public CasinoEntityType CasinoEntityType => CasinoEntityType.Slot;



    [SerializeField] private Node node;

    private readonly List<IVisitor> visitors = new();

    private IEnumerator timerGame;

    public void AddVisitor(IVisitor visitor)
    {
        if (!CanJoin || visitors.Contains(visitor))
            return;

        visitors.Add(visitor);
        visitor.OnEndDestination += OnVisitorReachedPoint;
        visitor.MoveTo(node, false);
    }

    public void RemoveVisitor(IVisitor visitor)
    {
        if (!visitors.Contains(visitor))
            return;

        visitor.OnEndDestination -= OnVisitorReachedPoint;
        visitors.Remove(visitor);
    }

    public bool ContainsVisitor(IVisitor visitor) => visitors.Contains(visitor);



    private void OnVisitorReachedPoint(INpc npc, Node node)
    {
        if(timerGame != null) Coroutines.Stop(timerGame);

        timerGame = Game(npc as IVisitor);
        Coroutines.Start(timerGame);
    }

    private IEnumerator Game(IVisitor visitor)
    {
        yield return new WaitForSeconds(0.2f);

        visitors[0].ActivatePlay();

        yield return new WaitForSeconds(1f);

        visitors[0].ActivateWin();

        yield return new WaitForSeconds(1.1f);

        visitors[0].ActivateIdle();

        yield return new WaitForSeconds(1);

        //OnVisitorRealised?.Invoke(visitor, this);
    }
}
