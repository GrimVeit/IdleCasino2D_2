using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEntityModel
{
    public bool IsOpen => true;
    public bool CanJoin => IsOpen && _songstress != null && _visitors.Count < _visitorNodes.Count;
    public int CountStaff => _songstress != null ? 1 : 0;

    private readonly Node _songstressNode;
    private readonly List<Node> _visitorNodes;
    private readonly List<IVisitor> _visitors = new();

    private ISongstress _songstress;
    private IEnumerator danceRoutine;

    public MusicEntityModel(List<Node> visitorNodes, Node songstressNode)
    {
        _songstressNode = songstressNode;
        _visitorNodes = visitorNodes;
    }

    public void Initialize()
    {
        // Запускаем постоянный цикл танца
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
        if (!CanJoin || _visitors.Contains(visitor))
            return;

        _visitors.Add(visitor);

        int index = _visitors.IndexOf(visitor);
        visitor.MoveTo(_visitorNodes[index], false);
        visitor.OnEndDestination += OnVisitorArrived;
    }

    public void RemoveVisitor(IVisitor visitor)
    {
        if (!_visitors.Contains(visitor))
            return;

        visitor.OnEndDestination -= OnVisitorArrived;
        _visitors.Remove(visitor);
    }

    private void OnVisitorArrived(INpc npc, Node node)
    {
        var visitor = npc as IVisitor;
        if (visitor == null) return;

        visitor.ActivateNpcRotation(NpcRotationEnum.BackLeft);
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
