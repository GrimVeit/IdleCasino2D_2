using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum VisitorState
{
    GoTo, At
}

public class BarEntityModel
{
    public bool CanJoin => _bartenderData.bartender != null && visitors.Count < _nodePlaceVisitors.Count;
    public int CountStaff => _bartenderData.bartender != null ? 1 : 0;

    private readonly List<Node> _nodePlaceVisitors;
    private readonly List<Node> _nodesPlaceStaff;

    private readonly Dictionary<IVisitor, VisitorState> visitors = new();

    private readonly ICasinoProfitStoreInfo _casinoProfitStoreInfo;

    private (IBartender bartender, MessagesBartenderType messagesType) _bartenderData;

    private IEnumerator messageVisitorRoutine;
    private IEnumerator messageDealerRoutine;

    private readonly Dictionary<IVisitor, int> visitorSlots = new();
    private readonly Dictionary<int, IEnumerator> slotRoutines = new();

    public BarEntityModel(
        ICasinoProfitStoreInfo casinoProfitStoreInfo,
        List<Node> nodePlaceVisitors,
        List<Node> nodesPlaceStaff)
    {
        _casinoProfitStoreInfo = casinoProfitStoreInfo;
        _nodePlaceVisitors = nodePlaceVisitors;
        _nodesPlaceStaff = nodesPlaceStaff;
    }

    public void Initialize() 
    {
        if (messageVisitorRoutine != null) Coroutines.Stop(messageVisitorRoutine);
        if (messageDealerRoutine != null) Coroutines.Stop(messageDealerRoutine);

        messageVisitorRoutine = RandomVisitorTalk();
        Coroutines.Start(messageVisitorRoutine);

        messageDealerRoutine = SingleBartenderTalk();
        Coroutines.Start(messageDealerRoutine);
    }
    public void Dispose() 
    {
        if (messageVisitorRoutine != null) Coroutines.Stop(messageVisitorRoutine);
        if (messageDealerRoutine != null) Coroutines.Stop(messageDealerRoutine);

        if (_bartenderData.bartender != null)
        {
            _bartenderData.bartender.OnClick -= BartenderClick;
            _bartenderData.bartender.Dispose();
        }
    }

    // ======================== STAFF ========================

    public void SetStaff(IStaff newStaff)
    {
        _bartenderData.bartender = newStaff as IBartender;

        if (_bartenderData.bartender == null)
            return;

        _bartenderData.bartender.OnClick += BartenderClick;
        _bartenderData.bartender.SetMove(_nodesPlaceStaff[0]);
        _bartenderData.bartender.Show();
        _bartenderData.bartender.ActivateAnimation(BartenderAnimationEnum.Idle);
        _bartenderData.bartender.ActivateNpcRotation(NpcRotationEnum.FrontRight);
        _bartenderData.messagesType = MessagesBartenderType.Idle;
    }

    // ======================== VISITOR ARRIVAL ========================

    private void OnVisitorDestination(INpc npc, Node node)
    {
        if (npc is not IVisitor visitor)
            return;

        visitors[visitor] = VisitorState.At;
        visitor.ActivateNpcRotation(NpcRotationEnum.BackLeft);

        if (!visitorSlots.ContainsKey(visitor))
            return;

        int slotIndex = visitorSlots[visitor];

        TryStartGame(visitor, slotIndex);
    }

    // ======================== GAMEPLAY ========================

    private void TryStartGame(IVisitor visitor, int slotIndex)
    {
        if (!CanStartGame(visitor))
            return;

        StartGame(visitor, slotIndex);
    }

    private bool CanStartGame(IVisitor visitor)
    {
        if (visitor == null)
            return false;

        if (_bartenderData.bartender == null)
            return false;

        return true;
    }

    private void StartGame(IVisitor visitor, int slotIndex)
    {
        if (slotRoutines.ContainsKey(slotIndex))
        {
            Coroutines.Stop(slotRoutines[slotIndex]);
        }

        IEnumerator routine = Game(visitor, slotIndex);

        slotRoutines[slotIndex] = routine;

        Coroutines.Start(routine);
    }

    private IEnumerator Game(IVisitor visitor, int slotIndex)
    {
        if (_bartenderData.bartender == null || visitor == null)
            yield break;

        Node staffNode = _nodesPlaceStaff[slotIndex];

        int indexCurrent = _nodesPlaceStaff.IndexOf(_bartenderData.bartender.CurrentNode);

        if(slotIndex > indexCurrent)
        {
            _bartenderData.bartender.ActivateNpcRotation(NpcRotationEnum.FrontLeft);
        }
        else if(slotIndex < indexCurrent)
        {
            _bartenderData.bartender.ActivateNpcRotation(NpcRotationEnum.FrontRight);
        }

        _bartenderData.bartender.MoveTo(staffNode, IsAbsolute: true);

        yield return new WaitForSeconds(0.5f);

        _bartenderData.bartender.ActivateNpcRotation(NpcRotationEnum.FrontRight);
        _bartenderData.bartender.ActivateAnimation(BartenderAnimationEnum.Work);
        _bartenderData.messagesType = MessagesBartenderType.Serving;
        SetMessageBartender(_bartenderData.bartender);

        yield return new WaitForSeconds(2f);

        _bartenderData.bartender.ActivateAnimation(BartenderAnimationEnum.Idle);
        _bartenderData.messagesType = MessagesBartenderType.Idle;

        yield return new WaitForSeconds(0.2f);

        visitor.ActivatePlay();

        float waitTime = UnityEngine.Random.Range(5f, 10f);
        yield return new WaitForSeconds(waitTime);

        visitor.ActivateIdle();

        AddProfit(visitor.Position, _casinoProfitStoreInfo.GetProfit(CasinoEntityType.Bar));

        OnVisitorRealised?.Invoke(visitor);

        RemoveVisitor(visitor);
    }

    // ======================== VISITOR TRAFFIC ========================

    public event Action<IVisitor> OnVisitorRealised;

    public void AddVisitor(IVisitor visitor)
    {
        if (!CanJoin || visitors.ContainsKey(visitor))
            return;

        int slotIndex = GetFreeSlot();

        if (slotIndex == -1)
            return;

        visitors.Add(visitor, VisitorState.GoTo);
        visitorSlots[visitor] = slotIndex;

        visitor.OnClick += VisitorClick;
        visitor.OnEndDestination += OnVisitorDestination;

        visitor.MoveTo(_nodePlaceVisitors[slotIndex], false);
    }

    public void RemoveVisitor(IVisitor visitor)
    {
        if (!visitors.ContainsKey(visitor))
            return;

        visitor.OnClick -= VisitorClick;
        visitor.OnEndDestination -= OnVisitorDestination;

        if (visitorSlots.TryGetValue(visitor, out int slot))
        {
            visitorSlots.Remove(visitor);

            if (slotRoutines.ContainsKey(slot))
            {
                Coroutines.Stop(slotRoutines[slot]);
                slotRoutines.Remove(slot);
            }
        }

        visitors.Remove(visitor);
    }

    private int GetFreeSlot()
    {
        List<int> freeSlots = new();

        for (int i = 0; i < _nodePlaceVisitors.Count; i++)
        {
            bool busy = visitorSlots.ContainsValue(i);

            if (!busy)
                freeSlots.Add(i);
        }

        if (freeSlots.Count == 0)
            return -1;

        int randomIndex = UnityEngine.Random.Range(0, freeSlots.Count);

        return freeSlots[randomIndex];
    }

    // ======================== PROFIT ========================

    public event Action<Vector3, int> OnAddCoins;

    private void AddProfit(Vector3 position, int amount)
    {
        OnAddCoins?.Invoke(position, amount);
    }

    //========================= VISITOR CLICK =================

    private void VisitorClick(IVisitor visitor)
    {
        SetMessageVisitor(visitor);
    }

    //========================== VISITOR MESSAGE ===============

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

            List<IVisitor> availableVisitors = new List<IVisitor>(visitors.Keys);

            for (int i = 0; i < talkCount && availableVisitors.Count > 0; i++)
            {
                int index = Random.Range(0, availableVisitors.Count);
                IVisitor visitor = availableVisitors[index];
                availableVisitors.RemoveAt(index);

                if (Random.value <= 0.7f)
                {
                    SetMessageVisitor(visitor);
                }

                yield return new WaitForSeconds(Random.Range(0.2f, 0.9f));
            }

            yield return new WaitForSeconds(Random.Range(2f, 6f));
        }
    }

    private void SetMessageVisitor(IVisitor visitor)
    {
        if (!visitors.TryGetValue(visitor, out var state))
            return;

        switch (state)
        {
            case VisitorState.GoTo:
                visitor.SetMessage(MessagesVisitor.GetRandomQuote(MessagesVisitorType.GoToBar));
                break;
            case VisitorState.At:
                visitor.SetMessage(MessagesVisitor.GetRandomQuote(MessagesVisitorType.AtBar));
                break;
        }
    }



    //========================== BARTENDER CLICK ===============

    private void BartenderClick(IBartender bartender)
    {
        SetMessageBartender(bartender);
    }



    //========================== BARTENDER MESSAGE ===============

    private IEnumerator SingleBartenderTalk()
    {
        while (true)
        {
            if (_bartenderData.bartender == null)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            if (Random.value <= 0.6f)
            {
                SetMessageBartender(_bartenderData.bartender);
            }

            yield return new WaitForSeconds(Random.Range(4f, 9f));
        }
    }

    private void SetMessageBartender(IBartender bartender)
    {
        if (bartender == null) return;

        bartender.SetMessage(MessagesBartender.GetRandomQuote(_bartenderData.messagesType));
    }
}
