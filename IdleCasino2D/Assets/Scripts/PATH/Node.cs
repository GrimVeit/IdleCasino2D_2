using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public NpcRotationEnum RotationEnum => rotationEnum;

    [SerializeField] private NpcRotationEnum rotationEnum;

    public Color colorSphere = Color.yellow;
    public Color colorLine = Color.green;
    public List<Node> neighbors = new List<Node>();

    private bool isActiveDraw = false; 

    private void Awake()
    {
        isActiveDraw = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = colorSphere;
        Gizmos.DrawSphere(transform.position, 0.2f);

        if (!isActiveDraw) return;

        Gizmos.color = colorLine;
        foreach (var n in neighbors)
        {
            if (n != null)
            {
                Vector3 dir = (n.transform.position - transform.position).normalized;
                Vector3 perp = Vector3.Cross(dir, Vector3.forward); // перпендикул€р дл€ смещени€
                Vector3 start = transform.position + perp * 0.05f;
                Vector3 end = n.transform.position + perp * 0.05f;
                Gizmos.DrawLine(start, end);
            }
        }
    }
}
