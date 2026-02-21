using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ExitEntity : MonoBehaviour
{
    public event Action<IVisitor> OnVisitorRealised;
    public int MaxSeats => int.MaxValue;
    public int OccupiedSeats => visitors.Count;
    public bool HasFreeSeats => true;
    public bool CanJoin => HasFreeSeats;

    public CasinoEntityType CasinoEntityType => CasinoEntityType.Exit;



    [SerializeField] private Node[] nodesExit;

    private readonly List<IVisitor> visitors = new();

    public void AddVisitor(IVisitor visitor)
    {
        if (visitors.Contains(visitor))
            return;

        visitors.Add(visitor);
        visitor.OnEndDestination += OnVisitorReachedPoint;
        var node = nodesExit[Random.Range(0, nodesExit.Length)];
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
        OnVisitorRealised?.Invoke(npc as IVisitor);
    }
}
