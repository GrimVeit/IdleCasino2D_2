using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ExitEntityModel
{
    public event Action<IVisitor> OnVisitorRealised;

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
        visitor.OnClick += ClickVisitor;
        visitor.OnEndDestination += OnVisitorReachedPoint;
        var node = _nodesExit[Random.Range(0, _nodesExit.Count)];
        visitor.MoveTo(node, false);
    }

    public void RemoveVisitor(IVisitor visitor)
    {
        if (!visitors.Contains(visitor))
            return;

        visitor.OnClick -= ClickVisitor;
        visitor.OnEndDestination -= OnVisitorReachedPoint;
        visitors.Remove(visitor);
    }



    private void OnVisitorReachedPoint(INpc npc, Node node)
    {
        OnVisitorRealised?.Invoke(npc as IVisitor);
    }

    #region VISITOR CLICK

    private void ClickVisitor(IVisitor visitor)
    {
        visitor.SetMessage(MessagesVisitor.GetRandomQuote(MessagesVisitorType.Exit));
    }

    #endregion
}