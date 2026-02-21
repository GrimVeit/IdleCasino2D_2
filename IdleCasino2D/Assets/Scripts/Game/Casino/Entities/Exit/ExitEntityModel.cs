using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ExitEntityModel
{
    public event Action<IVisitor> OnVisitorRealised;

    public int MaxSeats => 1;
    public int OccupiedSeats => visitors.Count;
    public bool HasFreeSeats => visitors.Count < MaxSeats;
    public bool CanJoin => true;

    private readonly List<Node> _nodesExit;
    private readonly List<IVisitor> visitors = new();
    public bool ContainsVisitor(IVisitor visitor) => visitors.Contains(visitor);

    public ExitEntityModel(List<Node> nodesExit)
    {
        _nodesExit = nodesExit;
    }

    public void AddVisitor(IVisitor visitor)
    {
        if (visitors.Contains(visitor))
            return;

        visitors.Add(visitor);
        visitor.OnEndDestination += OnVisitorReachedPoint;
        var node = _nodesExit[Random.Range(0, _nodesExit.Count)];
        visitor.MoveTo(node, false);
    }

    public void RemoveVisitor(IVisitor visitor)
    {
        if (!visitors.Contains(visitor))
            return;

        visitor.OnEndDestination -= OnVisitorReachedPoint;
        visitors.Remove(visitor);
    }



    private void OnVisitorReachedPoint(INpc npc, Node node)
    {
        OnVisitorRealised?.Invoke(npc as IVisitor);
    }
}