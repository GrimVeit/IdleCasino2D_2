using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntranceQueueEntity : MonoBehaviour
{
    public event Action<IVisitor> OnVisitorRealised;
    
    public CasinoEntityType CasinoEntityType => CasinoEntityType.EntranceQueue;



    [SerializeField] private Node[] queueNodes; // Точки очереди

    private readonly List<IVisitor> visitors = new List<IVisitor>();

    private IEnumerator timerQueue;

    public int MaxSeats => queueNodes.Length;
    public int OccupiedSeats => visitors.Count;
    public bool HasFreeSeats => visitors.Count < queueNodes.Length;
    public bool CanJoin => HasFreeSeats;

    public void AddVisitor(IVisitor visitor)
    {
        if (!CanJoin || visitors.Contains(visitor))
            return;

        visitors.Add(visitor);
        visitor.OnEndDestination += OnVisitorReachedPoint;

        UpdateQueuePositions(false);
    }

    public void RemoveVisitor(IVisitor visitor)
    {
        if (!visitors.Contains(visitor))
            return;

        visitor.OnEndDestination -= OnVisitorReachedPoint;
        visitors.Remove(visitor);

        UpdateQueuePositions(true);
    }

    public bool ContainsVisitor(IVisitor visitor) => visitors.Contains(visitor);

    private void UpdateQueuePositions(bool isWait)
    {
        if(timerQueue != null) Coroutines.Stop(timerQueue);

        timerQueue = UpdatePositionsCoro(isWait);
        Coroutines.Start(timerQueue);
    }

    private IEnumerator UpdatePositionsCoro(bool isWait)
    {
        if(isWait)
           yield return new WaitForSeconds(1f);

        for (int i = 0; i < visitors.Count && i < queueNodes.Length; i++)
        {
            if (visitors[i].CurrentNode != queueNodes[i])
            {
                visitors[i].MoveTo(queueNodes[i], true);
                yield return new WaitForSeconds(Random.Range(0.4f, 1f));
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void OnVisitorReachedPoint(INpc npc, Node node)
    {
        if (npc is not IVisitor visitor || visitors.Count == 0)
            return;

        if (visitors[0] != visitor)
            return;

        Coroutines.Start(Timer(visitor));
    }

    private IEnumerator Timer(IVisitor visitor)
    {
        yield return new WaitForSeconds(5);

        OnVisitorRealised?.Invoke(visitor);
    }
}
