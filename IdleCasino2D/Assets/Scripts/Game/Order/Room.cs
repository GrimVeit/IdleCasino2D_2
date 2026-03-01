using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Room
{
    public string roomName;
    public RectTransform[] vertices; // 4 точки ромба/зоны в Canvas
    public int orderMin;
    public int orderMax;
    [SerializeField] private List<OrderItem> staticObjs = new List<OrderItem>();
    public List<ISortable> staticObjects = new(); // Статические объекты комнаты

    public void Initialize()
    {
        staticObjects.AddRange(staticObjs);
    }

    // Проверка, находится ли точка внутри зоны
    public bool IsInside(Vector2 point)
    {
        if (vertices == null || vertices.Length < 3) return false;

        int j = vertices.Length - 1;
        bool inside = false;
        for (int i = 0; i < vertices.Length; j = i++)
        {
            Vector2 vi = vertices[i].position;
            Vector2 vj = vertices[j].position;

            if (((vi.y > point.y) != (vj.y > point.y)) &&
                (point.x < (vj.x - vi.x) * (point.y - vi.y) / (vj.y - vi.y) + vi.x))
            {
                inside = !inside;
            }
        }
        return inside;
    }

    public void DrawDebug(Color color)
    {
        if (vertices == null || vertices.Length < 2) return;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 start = vertices[i].position;
            Vector3 end = vertices[(i + 1) % vertices.Length].position;
            Debug.DrawLine(start, end, color);
        }
    }
}
