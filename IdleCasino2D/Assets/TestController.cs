using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    [SerializeField] private EntranceQueueEntity queueEntity;
    [SerializeField] private VisitorView visitorPrefab;
    [SerializeField] private Transform transformParent;
    [SerializeField] private Node spawnPoint;
 
    private List<IVisitor> spawnedVisitors = new List<IVisitor>();

    private void Update()
    {
        // Спавн нового NPC
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnVisitor();
        }

        // Удаление NPC по позиции 1..5
        if (Input.GetKeyDown(KeyCode.Alpha1)) RemoveVisitorAtIndex(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) RemoveVisitorAtIndex(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) RemoveVisitorAtIndex(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) RemoveVisitorAtIndex(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) RemoveVisitorAtIndex(4);
    }

    private void SpawnVisitor()
    {
        IVisitor newVisitor = SpawnVisitor(visitorPrefab, spawnPoint.transform);
        queueEntity.AddVisitor(newVisitor);
        spawnedVisitors.Add(newVisitor);

        Debug.Log($"[Spawn] Visitor added: {newVisitor}");
    }

    private void RemoveVisitorAtIndex(int index)
    {
        if (index < 0 || index >= queueEntity.OccupiedSeats)
        {
            Debug.LogWarning($"Нет визитора на позиции {index + 1}");
            return;
        }

        IVisitor visitorToRemove = queueEntity.ContainsVisitor(spawnedVisitors[index])
            ? spawnedVisitors[index]
            : null;

        if (visitorToRemove != null)
        {
            queueEntity.RemoveVisitor(visitorToRemove);
            spawnedVisitors.Remove(visitorToRemove);
            visitorToRemove.Destroy();

            Debug.Log($"[Remove] Visitor removed from position {index + 1}");
        }
    }

    public IVisitor SpawnVisitor(VisitorView visitorPrefab, Transform spawnPosition)
    {
        VisitorView view = Instantiate(visitorPrefab, transformParent);
        view.transform.SetLocalPositionAndRotation(spawnPosition.localPosition, visitorPrefab.transform.rotation);
        //VisitorModel model = new();
        VisitorPresenter presenter = new(null, view);
        presenter.Initialize();

        return presenter;
    }
}
