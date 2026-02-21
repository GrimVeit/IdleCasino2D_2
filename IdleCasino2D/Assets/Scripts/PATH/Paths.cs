using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Paths
{
    public static List<Node> FindPath(Node start, Node goal)
    {
        Debug.Log("Start find");

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();
        List<Node> frontier = new List<Node> { start };

        cameFrom[start] = null;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            // Выбираем узел с минимальной стоимостью
            Node current = frontier[0];
            foreach (var node in frontier)
                if (costSoFar[node] < costSoFar[current]) current = node;

            frontier.Remove(current);

            if (current == goal) break;

            foreach (Node next in current.neighbors)
            {
                float newCost = costSoFar[current] + Vector3.Distance(current.transform.position, next.transform.position);
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                    if (!frontier.Contains(next)) frontier.Add(next);
                }
            }
        }

        // Восстанавливаем путь
        List<Node> path = new List<Node>();
        Node temp = goal;
        while (temp != null)
        {
            path.Insert(0, temp);
            temp = cameFrom.ContainsKey(temp) ? cameFrom[temp] : null;
        }

        Debug.Log("Exit find");

        return path;
    }
}
