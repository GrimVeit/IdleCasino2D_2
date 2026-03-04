using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarEntityModel
{
    public bool IsOpen => true;
    public bool CanJoin => IsOpen &&_bartender != null && visitors.Count < _nodePlaceVisitors.Count;
    public int CountStaff => _bartender != null ? 1 : 0;

    private readonly List<Node> _nodePlaceVisitors;
    private readonly List<Node> _nodesPlaceStaff;
    private readonly List<IVisitor> visitors = new();

    private readonly ICasinoProfitStoreInfo _casinoProfitStoreInfo;
    private IBartender _bartender;
    private IEnumerator gameRoutine;

    public BarEntityModel(ICasinoProfitStoreInfo casinoProfitStoreInfo, List<Node> nodePlaceVisitors, List<Node> nodesPlaceStaff)
    {
        _casinoProfitStoreInfo = casinoProfitStoreInfo;
        _nodePlaceVisitors = nodePlaceVisitors;
        _nodesPlaceStaff = nodesPlaceStaff;
    }

    public void Initialize() { }
    public void Dispose() { }

    public void SetStaff(IStaff newStaff)
    {
        _bartender = newStaff as IBartender;
        if (_bartender == null) return;

        _bartender.SetMove(_nodesPlaceStaff[0]);
        _bartender.Show();
        _bartender.ActivateAnimation(BartenderAnimationEnum.Idle);
        _bartender.ActivateNpcRotation(NpcRotationEnum.FrontRight);
    }

    // ======================== GAMEPLAY ========================
    private void OnVisitorDestination(INpc npc, Node node)
    {
        npc.ActivateNpcRotation(NpcRotationEnum.BackLeft);

        if (_bartender != null)
        {
            TryStartGame(npc as IVisitor, auto: true);
        }
    }

    private void TryStartGame(IVisitor visitor, bool auto)
    {
        if (!CanStartGame(visitor, auto))
            return;

        StartGame(visitor);
    }

    private bool CanStartGame(IVisitor visitor, bool auto)
    {
        if (visitor == null)
            return false;

        if (!auto && _bartender != null)
            return false;

        return true;
    }

    private void StartGame(IVisitor visitor)
    {
        if (gameRoutine != null)
            Coroutines.Stop(gameRoutine);

        gameRoutine = Game(visitor);
        Coroutines.Start(gameRoutine);
    }

    private IEnumerator Game(IVisitor visitor)
    {
        if (_bartender == null || visitor == null)
            yield break;

        // Бармен идёт к точке и активирует анимацию работы
        _bartender.SetMove(_nodesPlaceStaff[0]);
        _bartender.ActivateAnimation(BartenderAnimationEnum.Work);

        // Работа бармена 2 секунды
        yield return new WaitForSeconds(2f);

        // Останавливаем анимацию бармена
        _bartender.ActivateAnimation(BartenderAnimationEnum.Idle);

        // Задержка перед анимацией визитора
        yield return new WaitForSeconds(0.2f);

        // Визитор играет
        visitor.ActivatePlay();

        // Случайное время ожидания 2-7 секунд
        float waitTime = UnityEngine.Random.Range(2f, 7f);
        yield return new WaitForSeconds(waitTime);

        visitor.ActivateIdle();

        AddProfit(visitor.Position, _casinoProfitStoreInfo.GetProfit(CasinoEntityType.Bar));
        // Событие завершения обслуживания
        OnVisitorRealised?.Invoke(visitor);

        // Убираем визитора
        RemoveVisitor(visitor);
    }

    // ======================== VISITOR TRAFFIC ========================
    public event Action<IVisitor> OnVisitorRealised;

    public void AddVisitor(IVisitor visitor)
    {
        if (!CanJoin || visitors.Contains(visitor))
            return;

        visitors.Add(visitor);
        visitor.OnEndDestination += OnVisitorDestination;
        // Можно добавить перемещение к свободной точке
        int index = visitors.IndexOf(visitor);
        visitor.MoveTo(_nodePlaceVisitors[index], false);
    }

    public void RemoveVisitor(IVisitor visitor)
    {
        if (!visitors.Contains(visitor))
            return;

        visitor.OnEndDestination -= OnVisitorDestination;
        visitors.Remove(visitor);
    }

    // ======================== PROFIT ========================
    #region PROFIT

    public event Action<Vector3, int> OnAddCoins;

    private void AddProfit(Vector3 position, int amount)
    {
        OnAddCoins?.Invoke(position, amount);
    }

    #endregion
}
