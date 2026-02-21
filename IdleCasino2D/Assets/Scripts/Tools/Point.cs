using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField] private bool isVisible = true;
    [SerializeField][Range(0, 1000)] private float size;
    [SerializeField] private Color color;
    [SerializeField] private DrawType drawType;


    private void OnDrawGizmos()
    {
        if(transform != null)
        {
            if (isVisible)
            {
                Gizmos.color = color;
                switch (drawType)
                {
                    case DrawType.Sphere:
                        Gizmos.DrawSphere(transform.position, size);
                        break;
                    case DrawType.Cube:
                        Gizmos.DrawCube(transform.position, new Vector3(size, size, size));
                        break;
                }
            }
        }
    }
}

public enum DrawType
{
    Sphere, Cube
}
