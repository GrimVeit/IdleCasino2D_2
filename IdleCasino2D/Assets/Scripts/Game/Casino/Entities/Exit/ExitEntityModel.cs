using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ExitEntityModel
{
    public event Action<IVisitor> OnVisitorRealised;

    private readonly List<Node> _nodesExit;
    private readonly List<IVisitor> visitors = new();

    private IEnumerator messageRoutine;

    public ExitEntityModel(List<Node> nodesExit)
    {
        _nodesExit = nodesExit;
    }

    public void Initialize()
    {
        if (messageRoutine != null) Coroutines.Stop(messageRoutine);

        messageRoutine = RandomVisitorTalk();
        Coroutines.Start(messageRoutine);
    }

    public void Dispose()
    {
        if (messageRoutine != null) Coroutines.Stop(messageRoutine);
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
        SetMessage(visitor);
    }

    #endregion

    #region VISITOR MESSAGE

    private IEnumerator RandomVisitorTalk()
    {
        while (true)
        {
            if (visitors.Count == 0)
            {
                yield return new WaitForSeconds(2f);
                continue;
            }

            int talkCount = Random.Range(1, visitors.Count + 1);

            List<IVisitor> availableVisitors = new List<IVisitor>(visitors);

            for (int i = 0; i < talkCount && availableVisitors.Count > 0; i++)
            {
                int index = Random.Range(0, availableVisitors.Count);
                IVisitor visitor = availableVisitors[index];
                availableVisitors.RemoveAt(index);

                if (Random.value <= 0.7f)
                {
                    SetMessage(visitor);
                }

                yield return new WaitForSeconds(Random.Range(0.2f, 0.9f));
            }

            // пауза перед следующей волной
            yield return new WaitForSeconds(Random.Range(2f, 6f));
        }
    }

    private void SetMessage(IVisitor visitor)
    {
        visitor.SetMessage(MessagesVisitor.GetRandomQuote(MessagesVisitorType.Exit));
    }

    #endregion
}