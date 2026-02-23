using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CasinoEntitiesSystem : MonoBehaviour
{
    [Header("EntranceQueue (first pos-->last pos)")]
    [SerializeField] private List<Node> nodesEntranceQueue;
    [SerializeField] private List<Node> nodesSlot;
    [SerializeField] private List<Node> nodesWheel;
    [SerializeField] private List<Node> nodesPoker;
    [SerializeField] private List<Node> nodesExit;

    [SerializeField] private ViewContainer viewContainer_World;

    public Room[] rooms;

    private ICasinoEntity enterEntity;
    private ICasinoEntity exitEntity;
    private List<ICasinoEntity> casinoEntities = new();
    private List<IVisitor> _visitors = new();

    public void Awake()
    {
        viewContainer_World.Initialize();

        var entityEnter = new EntranceQueueEntityPresenter(new EntranceQueueEntityModel(nodesEntranceQueue));
        entityEnter.Initialize();
        enterEntity = entityEnter;
        casinoEntities.Add(entityEnter);

        var entityExit = new ExitEntityPresenter(new ExitEntityModel(nodesExit));
        entityExit.Initialize();
        exitEntity = entityExit;
        casinoEntities.Add(entityExit);

        for (int i = 0; i < 6; i++)
        {
            var spot = new SlotSpotPresenter(new SlotSpotModel(), viewContainer_World.GetView<SlotSpotView>($"Slot_{i+1}"));
            spot.Initialize();

            var entity = new SlotMachineEntityPresenter(new SlotMachineEntityModel(spot, nodesSlot[i]));
            entity.Initialize();

            casinoEntities.Add(entity);
        }

        for (int i = 0; i < 6; i++)
        {
            var spot = new WheelSpotPresenter(new WheelSpotModel(), viewContainer_World.GetView<WheelSpotView>($"Wheel_{i + 1}"));
            spot.Initialize();

            var entity = new WheelEntityPresenter(new WheelEntityModel(spot, nodesWheel[i]));
            entity.Initialize();

            casinoEntities.Add(entity);
        }

        for (int i = 0; i < 4; i++)
        {
            var spot = new PokerSpotPresenter(new PokerSpotModel(), viewContainer_World.GetView<PokerSpotView>($"Poker_{i + 1}"));
            spot.Initialize();

            var entity = new PokerEntityPresenter(new PokerEntityModel(spot, nodesPoker[i]));
            entity.Initialize();

            casinoEntities.Add(entity);
        }
    }

    private void Start()
    {
        casinoEntities.ForEach(e => e.OnVisitorRealised += EntityRelease);
    }

    private void OnDestroy()
    {
        casinoEntities.ForEach(e => e.OnVisitorRealised -= EntityRelease);
    }

    private void EntityRelease(IVisitor visitor, ICasinoEntity entity)
    {
        entity.RemoveVisitor(visitor);

        if(entity.CasinoEntityType == CasinoEntityType.Exit)
        {
            Debug.Log("Удаление персонажа");
            RemoveVisitorCompletely(visitor);
            return;
        }

        if (visitor.MoveNextStep())
        {
            Debug.Log("Пытаемся дать новую цель");
            TryAssign(visitor);
        }
        else
        {
            Debug.Log("Целей больше нет, отправляем на удаление");
            exitEntity.AddVisitor(visitor);
        }
    }

    private void TryAssign(IVisitor visitor)
    {
        var targetType = visitor.CurrentTarget;

        var entities = casinoEntities
            .Where(e => e.CasinoEntityType == targetType && e.CanJoin)
            .OrderBy(e => e.OccupiedSeats).ToList();



        if(entities.Count != 0)
        {
            var entity = entities[Random.Range(0, entities.Count)];
            Debug.Log("Дали новую цель - " + targetType.ToString());
            entity.AddVisitor(visitor);
        }
        else
        {
            Debug.Log("Цель была занята, отправляем на удаление");
            exitEntity.AddVisitor(visitor);
        }
    }

    private void RemoveVisitorCompletely(IVisitor visitor)
    {
        _visitors.Remove(visitor);

        visitor.Destroy();
    }
}
