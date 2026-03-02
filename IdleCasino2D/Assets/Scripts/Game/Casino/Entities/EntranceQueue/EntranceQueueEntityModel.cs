using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntranceQueueEntityModel
{
    public event Action<IVisitor> OnVisitorRealised;
    public bool CanJoin => visitors.Count < _queueNodes.Count;



    private readonly List<Node> _queueNodes;
    private readonly List<IVisitor> visitors = new List<IVisitor>();
    private IEnumerator timerQueue;

    public EntranceQueueEntityModel(List<Node> queueNodes)
    {
        _queueNodes = queueNodes;
    }

    public void AddVisitor(IVisitor visitor)
    {
        if (!CanJoin || visitors.Contains(visitor))
            return;

        visitors.Add(visitor);
        visitor.OnClick += ClickVisitor;
        visitor.OnEndDestination += OnVisitorReachedPoint;

        UpdateQueuePositions(false);
    }

    public void RemoveVisitor(IVisitor visitor)
    {
        if (!visitors.Contains(visitor))
            return;

        visitor.OnClick -= ClickVisitor;
        visitor.OnEndDestination -= OnVisitorReachedPoint;
        visitors.Remove(visitor);

        UpdateQueuePositions(true);
    }

    public bool ContainsVisitor(IVisitor visitor) => visitors.Contains(visitor);

    private void UpdateQueuePositions(bool isWait)
    {
        if (timerQueue != null) Coroutines.Stop(timerQueue);

        timerQueue = UpdatePositionsCoro(isWait);
        Coroutines.Start(timerQueue);
    }

    private IEnumerator UpdatePositionsCoro(bool isWait)
    {
        if (isWait)
            yield return new WaitForSeconds(1f);

        for (int i = 0; i < visitors.Count && i < _queueNodes.Count; i++)
        {
            if (visitors[i].CurrentNode != _queueNodes[i])
            {
                visitors[i].MoveTo(_queueNodes[i], true);
                yield return new WaitForSeconds(Random.Range(0.4f, 1f));
            }
            else
            {
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    private void OnVisitorReachedPoint(INpc npc, Node node)
    {
        if (npc is not IVisitor visitor || visitors.Count == 0)
            return;

        if (visitors[0] != visitor)
            return;

        OnVisitorRealised?.Invoke(visitor);
    }

    #region VISITOR CLICK

    public event Action<IVisitor> OnClickVisitor;

    private void ClickVisitor(IVisitor visitor)
    {
        OnClickVisitor?.Invoke(visitor);
    }

    #endregion
}
