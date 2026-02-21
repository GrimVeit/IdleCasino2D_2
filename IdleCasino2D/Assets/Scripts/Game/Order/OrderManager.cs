using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private VisitorView visitorPrefab;
    [SerializeField] private Transform transformParentVisitors;
    [SerializeField] private Transform traspawnPoint;

    public Room[] rooms;                     // Все комнаты
    public List<ISortable> dynamicObjects;
    // NPC, игроки и любые ISortable

    void LateUpdate()
    {
        foreach (var room in rooms)
        {
            // Собираем все объекты комнаты
            List<ISortable> allObjects = new List<ISortable>();

            // Статические объекты конвертируем в временные ISortable для сортировки
            if (room.staticObjects != null)
            {
                foreach (var sr in room.staticObjects)
                {
                    //allObjects.Add(new SpriteSortableWrapper(sr));
                }
            }

            // Динамические объекты, которые находятся в комнате
            foreach (var obj in dynamicObjects)
            {
                if (room.IsInside(obj.Position))
                    allObjects.Add(obj);
            }

            // Сортировка по Y (ближе к камере = выше Order)
            allObjects.Sort((a, b) => b.Position.y.CompareTo(a.Position.y));

            // Назначаем Order
            int order = room.orderMax;
            foreach (var obj in allObjects)
            {
                obj.SetOrder(order);
                order--;
                if (order < room.orderMin) order = room.orderMin;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (rooms == null) return;
        Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow };
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i] != null)
                rooms[i].DrawDebug(colors[i % colors.Length]);
        }
    }
}

public interface ISortable
{
    Vector3 Position { get; }
    void SetOrder(int order);
}
