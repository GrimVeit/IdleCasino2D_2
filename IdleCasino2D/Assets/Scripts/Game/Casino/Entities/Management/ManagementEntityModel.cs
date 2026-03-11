using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManagementEntityModel
{
    private bool CanJoinVisitor => managers.Count < _nodesManagerPositions.Count;
    public int CountStaff => managers.Count;

    private readonly List<Node> _nodesManagerPositions;
    private readonly List<IManager> managers = new();
    private readonly Dictionary<IManager, IEnumerator> managerRoutines = new();

    public ManagementEntityModel(List<Node> nodesManagerPositions)
    {
        _nodesManagerPositions = nodesManagerPositions;
    }

    public void Initialize() { }
    public void Dispose()
    {
        // Останавливаем все корутины при уничтожении
        foreach (var routine in managerRoutines.Values)
        {
            if (routine != null)
                Coroutines.Stop(routine);
        }
        managerRoutines.Clear();
    }

    public void SetStaff(IStaff newManager)
    {
        if (!CanJoinVisitor || managers.Contains(newManager))
            return;

        var manager = newManager as IManager;

        managers.Add(manager);

        // Выбираем случайное свободное место
        var freeNodes = _nodesManagerPositions
            .Where(n => !managers.Any(m => m.CurrentNode == n))
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
            yield return new WaitForSeconds(Random.Range(5f, 10f));

            // Gesturing 3-6 секунд
            manager.ActivateAnimation(ManagerAnimationEnum.Gesture);
            yield return new WaitForSeconds(Random.Range(3f, 6f));
        }
    }
}
