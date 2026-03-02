using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicEntityModel
{
    public bool IsOpen => true;
    public bool CanJoin => IsOpen && _songstress != null && _occupiedNodes.Count < _visitorNodes.Count;
    public int CountStaff => _songstress != null ? 1 : 0;

    private readonly Node _songstressNode;
    private readonly List<Node> _visitorNodes;

    private readonly Dictionary<IVisitor, Node> _occupiedNodes = new();

    private ISongstress _songstress;
    private IEnumerator danceRoutine;

    public MusicEntityModel(List<Node> visitorNodes, Node songstressNode)
    {
        _songstressNode = songstressNode;
        _visitorNodes = visitorNodes;
    }

    public void Initialize()
    {
        if (_songstress != null)
        {
            danceRoutine = DanceCycle();
            Coroutines.Start(danceRoutine);
        }
    }

    public void Dispose()
    {
        if (danceRoutine != null)
            Coroutines.Stop(danceRoutine);
    }

    #region STAFF

    public void SetStaff(IStaff staff)
    {
        _songstress = staff as ISongstress;
        if (_songstress == null) return;

        _songstress.Show();
        _songstress.SetMove(_songstressNode);
        _songstress.ActivateNpcRotation(NpcRotationEnum.FrontLeft);

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
            _songstress.ActivateAnimation(SongstressAnimationEnum.Idle);
            yield return new WaitForSeconds(UnityEngine.Random.Range(4f, 8f));

            _songstress.ActivateAnimation(SongstressAnimationEnum.Song);
            yield return new WaitForSeconds(UnityEngine.Random.Range(12f, 20f));
        }
    }

    #endregion

    #region VISITORS

    public event Action<IVisitor> OnVisitorRealised;
    public event Action<Vector3, int> OnAddCoins;

    public void AddVisitor(IVisitor visitor)
    {
        if (!CanJoin || _occupiedNodes.ContainsKey(visitor))
            return;

        // Получаем список свободных нод
        var freeNodes = _visitorNodes
            .Where(n => !_occupiedNodes.ContainsValue(n))
            .ToList();

        if (freeNodes.Count == 0)
            return;

        // Выбираем случайную свободную точку
        Node randomNode = freeNodes[UnityEngine.Random.Range(0, freeNodes.Count)];

        _occupiedNodes[visitor] = randomNode;

        visitor.MoveTo(randomNode, false);
        visitor.OnEndDestination += OnVisitorArrived;
    }

    public void RemoveVisitor(IVisitor visitor)
    {
        if (!_occupiedNodes.ContainsKey(visitor))
            return;

        visitor.OnEndDestination -= OnVisitorArrived;

        _occupiedNodes.Remove(visitor);
    }

    private void OnVisitorArrived(INpc npc, Node node)
    {
        var visitor = npc as IVisitor;
        if (visitor == null)
            return;

        if (!_occupiedNodes.TryGetValue(visitor, out var visitorNode))
            return;

        int index = _visitorNodes.IndexOf(visitorNode);

        switch (index)
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

        Coroutines.Start(VisitorRoutine(visitor));
    }

    private IEnumerator VisitorRoutine(IVisitor visitor)
    {
        float waitTime = UnityEngine.Random.Range(4f, 10f);
        yield return new WaitForSeconds(waitTime);

        // 20% шанс оставить деньги
        if (UnityEngine.Random.value <= 0.2f)
        {
            int reward = UnityEngine.Random.Range(5, 15);
            OnAddCoins?.Invoke(visitor.Position, reward);
        }

        OnVisitorRealised?.Invoke(visitor);
        RemoveVisitor(visitor);
    }

    #endregion
}
