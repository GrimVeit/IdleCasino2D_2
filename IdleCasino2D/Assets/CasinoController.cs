using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CasinoController : MonoBehaviour
{
    [SerializeField] private VisitorView visitorPrefab;
    [SerializeField] private Transform transformParentVisitors;
    [SerializeField] private Transform traspawnPoint;

    [Header("EntranceQueue (first pos-->last pos)")]
    [SerializeField] private List<Node> nodesEntranceQueue;

    [Header("Slot")]
    [SerializeField] private List<Node> nodesSlot;

    [Header("Wheel")]
    [SerializeField] private List<Node> nodesWheel;

    [Header("Poker")]
    [SerializeField] private List<Node> nodesPoker;

    [Header("Exit")]
    [SerializeField] private List<Node> nodesExit;



    [SerializeField] private ViewContainer viewContainer_World;
    [SerializeField] private ViewContainer viewContainer_UI;

    private MapOrderPresenter mapOrderPresenter;

    public Room[] rooms;

    private ICasinoEntity enterEntity;
    private ICasinoEntity exitEntity;
    private List<ICasinoEntity> casinoEntities = new();
    private List<IVisitor> _visitors = new();

    private List<List<CasinoEntityType>> routesVisisitor = new()
    {
        //new List<CasinoEntityType> {CasinoEntityType.EntranceQueue, CasinoEntityType.Wheel, CasinoEntityType.Exit },
        new List<CasinoEntityType> {CasinoEntityType.EntranceQueue, CasinoEntityType.Slot, CasinoEntityType.Exit },
        //new List<CasinoEntityType> {CasinoEntityType.EntranceQueue, CasinoEntityType.Poker, CasinoEntityType.Exit }
    };

    private BankPresenter bankPresenter;
    private CoinSystemPresenter coinSystemPresenter;
    private TouchCameraPresenter touchCameraPresenter;

    public void Awake()
    {
        viewContainer_UI.Initialize();
        viewContainer_World.Initialize();


        bankPresenter = new BankPresenter(new BankModel(), viewContainer_UI.GetView<BankView>());
        bankPresenter.Initialize();

        touchCameraPresenter = new TouchCameraPresenter(viewContainer_UI.GetView<TouchCameraView>());
        touchCameraPresenter.Initialize();
        touchCameraPresenter.ActivateInteractive();

        coinSystemPresenter = new CoinSystemPresenter(new CoinSystemModel(bankPresenter), viewContainer_UI.GetView<CoinSystemView>());
        coinSystemPresenter.Initialize();

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

            var entity = new SlotMachineEntityPresenter(new SlotMachineEntityModel(spot, nodesSlot[i], coinSystemPresenter));
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

        mapOrderPresenter = new MapOrderPresenter(new MapOrderModel(rooms, _visitors.Cast<ISortable>().ToList()));
        mapOrderPresenter.Initialize();
    }

    private void Start()
    {
        casinoEntities.ForEach(e => e.OnVisitorRealised += EntityRelease);

        Coroutines.Start(SpawnRoutine());
    }

    void LateUpdate()
    {
        foreach (var room in rooms)
        {
            List<ISortable> allObjects = new List<ISortable>();

            if (room.staticObjects != null)
            {
                foreach (var sr in room.staticObjects)
                {
                    allObjects.Add(sr);
                }
            }

            foreach (var visitor in _visitors)
            {
                if (room.IsInside(visitor.Position))
                    allObjects.Add(visitor);
            }

            allObjects.Sort((a, b) => a.Position.y.CompareTo(b.Position.y));
            int order = room.orderMax;
            foreach (var obj in allObjects)
            {
                obj.SetOrder(order);
                order--;
                if (order < room.orderMin) order = room.orderMin;
            }
        }
    }

    private void OnDestroy()
    {
        casinoEntities.ForEach(e => e.OnVisitorRealised -= EntityRelease);
    }

    private void OnDrawGizmos()
    {
        //if (rooms == null) return;
        //Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow };
        //for (int i = 0; i < rooms.Length; i++)
        //{
        //    if (rooms[i] != null)
        //        rooms[i].DrawDebug(colors[i % colors.Length]);
        //}
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if(_visitors.Count < 100 && enterEntity.CanJoin)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SpawnVisitor();
                }
            }

            yield return null;
        }
    }

    private void SpawnVisitor()
    {
        var view = Instantiate(visitorPrefab, transformParentVisitors);
        view.transform.SetLocalPositionAndRotation(traspawnPoint.localPosition, visitorPrefab.transform.rotation);

        var presenter = new VisitorPresenter(new VisitorModel(routesVisisitor[Random.Range(0, routesVisisitor.Count)]), view);
        presenter.Initialize();
        IVisitor visitor = presenter;

        _visitors.Add(visitor);
        TryAssign(visitor);
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
