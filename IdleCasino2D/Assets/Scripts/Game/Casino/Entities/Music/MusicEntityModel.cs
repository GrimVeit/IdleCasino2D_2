using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicEntityModel
{
    public bool IsOpen => true;
    public bool CanJoin => _songstressData.songstress != null && visitors.Count < _visitorNodes.Count;
    public int CountStaff => _songstressData.songstress != null ? 1 : 0;

    private readonly Node _songstressNode;
    private readonly List<Node> _visitorNodes;

    private readonly Dictionary<IVisitor, VisitorState> visitors = new();

    private readonly Dictionary<IVisitor, int> visitorSlots = new();

    private readonly ICasinoProfitStoreInfo _casinoProfitStoreInfo;

    private (ISongstress songstress, MessagesSongstressType messagesType) _songstressData;

    private IEnumerator messageRoutine;
    private IEnumerator danceRoutine;

    public MusicEntityModel(
        ICasinoProfitStoreInfo casinoProfitStoreInfo,
        List<Node> visitorNodes,
        Node songstressNode)
    {
        _casinoProfitStoreInfo = casinoProfitStoreInfo;
        _visitorNodes = visitorNodes;
        _songstressNode = songstressNode;
    }

    public void Initialize()
    {
        if (_songstressData.songstress != null && danceRoutine == null)
        {
            danceRoutine = DanceCycle();
            Coroutines.Start(danceRoutine);
        }

        if (messageRoutine != null) Coroutines.Stop(messageRoutine);

        messageRoutine = RandomVisitorTalk();
        Coroutines.Start(messageRoutine);
    }

    public void Dispose()
    {
        if (danceRoutine != null) Coroutines.Stop(danceRoutine);
        if (messageRoutine != null) Coroutines.Stop(messageRoutine);

        if(_songstressData.songstress != null)
        {
            _songstressData.songstress.OnClick -= SongstressClick;
        }
    }

    #region STAFF

    public void SetStaff(IStaff staff)
    {
        _songstressData.songstress = staff as ISongstress;

        if (_songstressData.songstress == null)
            return;

        _songstressData.songstress.Show();
        _songstressData.songstress.OnClick += SongstressClick;
        _songstressData.songstress.SetMove(_songstressNode);
        _songstressData.songstress.ActivateNpcRotation(NpcRotationEnum.FrontLeft);
        _songstressData.messagesType = MessagesSongstressType.Idle;

        if (danceRoutine == null)
        {
            danceRoutine = DanceCycle();
            Coroutines.Start(danceRoutine);
        }
    }

    private IEnumerator DanceCycle()
    {
        while (true)
        {
            _songstressData.songstress.ActivateAnimation(SongstressAnimationEnum.Idle);
            _songstressData.messagesType = MessagesSongstressType.Idle;
            yield return new WaitForSeconds(Random.Range(4f, 9f));


            _songstressData.songstress.ActivateAnimation(SongstressAnimationEnum.Song);
            _songstressData.messagesType = MessagesSongstressType.Performing;
            int cyclesSong = Random.Range(6, 12);

            for (int i = 0; i < cyclesSong; i++)
            {
                SetMessageSongstressRandomTurn(_songstressData.songstress, false);
                yield return new WaitForSeconds(Random.Range(1f, 2f));
            }
        }
    }

    #endregion

    #region VISITORS

    public event Action<IVisitor> OnVisitorRealised;
    public event Action<Vector3, int> OnAddCoins;

    public void AddVisitor(IVisitor visitor)
    {
        if (!CanJoin || visitors.ContainsKey(visitor))
            return;

        int slotIndex = GetFreeSlot();

        if (slotIndex == -1)
            return;

        visitors[visitor] = VisitorState.GoTo;
        visitorSlots[visitor] = slotIndex;

        visitor.OnClick += VisitorClick;
        visitor.OnEndDestination += OnVisitorArrived;

        visitor.MoveTo(_visitorNodes[slotIndex], false);
    }

    public void RemoveVisitor(IVisitor visitor)
    {
        if (!visitors.ContainsKey(visitor))
            return;

        visitor.OnClick -= VisitorClick;
        visitor.OnEndDestination -= OnVisitorArrived;

        visitors.Remove(visitor);
        visitorSlots.Remove(visitor);
    }

    private int GetFreeSlot()
    {
        List<int> freeSlots = new();

        for (int i = 0; i < _visitorNodes.Count; i++)
        {
            bool busy = visitorSlots.ContainsValue(i);

            if (!busy)
                freeSlots.Add(i);
        }

        if (freeSlots.Count == 0)
            return -1;

        int randomIndex = Random.Range(0, freeSlots.Count);

        return freeSlots[randomIndex];
    }

    private void OnVisitorArrived(INpc npc, Node node)
    {
        IVisitor visitor = npc as IVisitor;

        if (visitor == null)
            return;

        if (!visitorSlots.ContainsKey(visitor))
            return;

        int slotIndex = visitorSlots[visitor];

        visitors[visitor] = VisitorState.At;

        RotateVisitor(visitor, slotIndex);

        Coroutines.Start(VisitorRoutine(visitor));
    }

    private void RotateVisitor(IVisitor visitor, int slotIndex)
    {
        switch (slotIndex)
        {
            case 0:
                visitor.ActivateNpcRotation(NpcRotationEnum.FrontRight);
                break;

            case 1:
                visitor.ActivateNpcRotation(NpcRotationEnum.BackRight);
                break;

            case 2:
                visitor.ActivateNpcRotation(NpcRotationEnum.BackLeft);
                break;

            default:
                visitor.ActivateNpcRotation(NpcRotationEnum.BackLeft);
                break;
        }
    }

    private IEnumerator VisitorRoutine(IVisitor visitor)
    {
        float waitTime = Random.Range(4f, 10f);
        yield return new WaitForSeconds(waitTime);

        if (Random.value <= 0.2f)
        {
            OnAddCoins?.Invoke(
                visitor.Position,
                _casinoProfitStoreInfo.GetProfit(CasinoEntityType.Music));
        }

        OnVisitorRealised?.Invoke(visitor);

        RemoveVisitor(visitor);
    }

    #endregion

    #region VISITOR CLICK

    private void VisitorClick(IVisitor visitor)
    {
        SetMessageVisitor(visitor, true);
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

            List<IVisitor> availableVisitors = new List<IVisitor>(visitors.Keys);

            for (int i = 0; i < talkCount && availableVisitors.Count > 0; i++)
            {
                int index = Random.Range(0, availableVisitors.Count);
                IVisitor visitor = availableVisitors[index];
                availableVisitors.RemoveAt(index);

                if (Random.value <= 0.7f)
                {
                    SetMessageVisitor(visitor, false);
                }

                yield return new WaitForSeconds(Random.Range(0.2f, 0.9f));
            }

            yield return new WaitForSeconds(Random.Range(2f, 6f));
        }
    }

    private void SetMessageVisitor(IVisitor visitor, bool isSound = false)
    {
        if (!visitors.TryGetValue(visitor, out var state))
            return;

        switch (state)
        {
            case VisitorState.GoTo:
                visitor.SetMessage(MessagesVisitor.GetRandomQuote(MessagesVisitorType.GoToSinger), isSound);
                break;

            case VisitorState.At:
                visitor.SetMessage(MessagesVisitor.GetRandomQuote(MessagesVisitorType.AtSinger), isSound);
                break;
        }
    }

    #endregion

    #region SONGSTRESS CLICK

    private void SongstressClick(ISongstress songstress)
    {
        SetMessageSongstressRandomTurn(songstress, true);
    }

    #endregion

    #region MESSAGE

    private void SetMessageSongstressRandomTurn(ISongstress songstress, bool isSound)
    {
        if (songstress == null) return;

        songstress.SetMessage(MessagesSongstress.GetRandomQuote(_songstressData.messagesType), isSound);
    }

    #endregion
}
