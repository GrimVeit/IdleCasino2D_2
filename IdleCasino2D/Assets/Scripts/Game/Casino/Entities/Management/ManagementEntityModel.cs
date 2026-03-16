using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManagementEntityModel
{
    private bool CanJoinVisitor => managers.Count < _nodesManagerPositions.Count;
    public int CountStaff => managers.Count;

    private readonly List<Node> _nodesManagerPositions;
    private readonly Dictionary<IManager, MessagesManagerType> managers = new();
    private readonly Dictionary<IManager, IEnumerator> managerRoutines = new();

    private IEnumerator messageRoutine;

    public ManagementEntityModel(List<Node> nodesManagerPositions)
    {
        _nodesManagerPositions = nodesManagerPositions;
    }

    public void Initialize() 
    {
        if (messageRoutine != null) Coroutines.Stop(messageRoutine);

        messageRoutine = RandomVisitorTalk();
        Coroutines.Start(messageRoutine);
    }
    public void Dispose()
    {
        foreach (var manager in managers.Keys)
        {
            manager.OnClick -= ManagerClick;
            manager.Dispose();
        }

        // Останавливаем все корутины при уничтожении
        foreach (var routine in managerRoutines.Values)
        {
            if (routine != null)
                Coroutines.Stop(routine);
        }
        managerRoutines.Clear();

        if (messageRoutine != null) Coroutines.Stop(messageRoutine);
    }

    public void SetStaff(IStaff newManager)
    {
        if (!CanJoinVisitor || managers.Keys.Contains(newManager))
            return;

        var manager = newManager as IManager;
        manager.OnClick += ManagerClick;
        managers.Add(manager, MessagesManagerType.Idle);

        // Выбираем случайное свободное место
        var freeNodes = _nodesManagerPositions
            .Where(n => !managers.Keys.Any(m => m.CurrentNode == n))
            .ToList();

        if (freeNodes.Count == 0)
            return;

        Node targetNode = freeNodes[Random.Range(0, freeNodes.Count)];


        manager.SetMove(targetNode); // просто ставим на место
        manager.ActivateNpcRotation(targetNode.RotationEnum);
        manager.Show();
        manager.ActivateAnimation(ManagerAnimationEnum.Idle);

        var routine = ManagerAnimationRoutine(manager);
        Coroutines.Start(routine);
        managerRoutines[manager] = routine;
    }

    private IEnumerator ManagerAnimationRoutine(IManager manager)
    {
        while (true)
        {
            // Idle 5-10 секунд
            manager.ActivateAnimation(ManagerAnimationEnum.Idle);
            managers[manager] = MessagesManagerType.Idle;
            yield return new WaitForSeconds(Random.Range(5f, 10f));

            // Gesturing 3-6 секунд
            manager.ActivateAnimation(ManagerAnimationEnum.Gesture);
            managers[manager] = MessagesManagerType.RadioTalk;
            SetMessage(manager);
            yield return new WaitForSeconds(Random.Range(3f, 6f));
        }
    }

    #region MANAGER CLICK

    private void ManagerClick(IManager manager)
    {
        SetMessage(manager);
    }

    #endregion

    #region MESSAGE

    private IEnumerator RandomVisitorTalk()
    {
        while (true)
        {
            if (managers.Count == 0)
            {
                yield return new WaitForSeconds(2f);
                continue;
            }
            int talkCount = Random.Range(1, managers.Count + 1);

            List<IManager> availableManagers = new List<IManager>(managers.Keys);

            for (int i = 0; i < talkCount && availableManagers.Count > 0; i++)
            {
                int index = Random.Range(0, availableManagers.Count);
                IManager manager = availableManagers[index];
                availableManagers.RemoveAt(index);

                if (Random.value <= 0.7f)
                {
                    SetMessage(manager);
                }

                yield return new WaitForSeconds(Random.Range(0.4f, 1.5f));
            }

            yield return new WaitForSeconds(Random.Range(2f, 6f));
        }
    }

    private void SetMessage(IManager manager)
    {
        if (manager == null) return;

        manager.SetMessage(MessagesManager.GetRandomQuote(managers[manager]));
    }

    private void SetMessageTurn(IManager manager, SpeechTurnEnum speechTurnEnum)
    {
        if (manager == null) return;

        manager.SetMessage(MessagesManager.GetRandomQuote(managers[manager]), speechTurnEnum);
    }

    #endregion
}
