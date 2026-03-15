using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VisitorState
{
    GoTo, At
}

public class BarEntityModel
{
    public bool CanJoin => _bartender != null && visitors.Count < _nodePlaceVisitors.Count;
    public int CountStaff => _bartender != null ? 1 : 0;

    private readonly List<Node> _nodePlaceVisitors;
    private readonly List<Node> _nodesPlaceStaff;

    private readonly Dictionary<IVisitor, VisitorState> visitors = new();

    private readonly ICasinoProfitStoreInfo _casinoProfitStoreInfo;

    private IBartender _bartender;

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

    public void Initialize() { }
    public void Dispose() { }

    // ======================== STAFF ========================

    public void SetStaff(IStaff newStaff)
    {
        _bartender = newStaff as IBartender;

        if (_bartender == null)
            return;

        _bartender.SetMove(_nodesPlaceStaff[0]);
        _bartender.Show();
        _bartender.ActivateAnimation(BartenderAnimationEnum.Idle);
        _bartender.ActivateNpcRotation(NpcRotationEnum.FrontRight);
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

        if (_bartender == null)
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
        if (_bartender == null || visitor == null)
            yield break;

        Node staffNode = _nodesPlaceStaff[slotIndex];

        int indexCurrent = _nodesPlaceStaff.IndexOf(_bartender.CurrentNode);

        if(slotIndex > indexCurrent)
        {
            _bartender.ActivateNpcRotation(NpcRotationEnum.FrontLeft);
        }
        else if(slotIndex < indexCurrent)
        {
            _bartender.ActivateNpcRotation(NpcRotationEnum.FrontRight);
        }

        _bartender.MoveTo(staffNode, IsAbsolute: true);

        yield return new WaitForSeconds(0.5f);

        _bartender.ActivateNpcRotation(NpcRotationEnum.FrontRight);
        _bartender.ActivateAnimation(BartenderAnimationEnum.Work);

        yield return new WaitForSeconds(2f);

        _bartender.ActivateAnimation(BartenderAnimationEnum.Idle);

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
}
