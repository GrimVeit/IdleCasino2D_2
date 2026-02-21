using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMatch : MonoBehaviour
{
    public float minMatch = 0.3f;
    public float maxMatch = 0.7f;

    private CanvasScaler scaler;
    private Vector2 lastRes;

    void Start()
    {
        scaler = GetComponent<CanvasScaler>();
        ApplyMatch();
        lastRes = new Vector2(Screen.width, Screen.height);
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Screen.width != lastRes.x || Screen.height != lastRes.y)
        {
            ApplyMatch();
            lastRes = new Vector2(Screen.width, Screen.height);
        }
    }
#endif

    void ApplyMatch()
    {
        float current = (float)Screen.width / Screen.height;
        float baseRatio = 1080f / 1920f; // 16:9

        // Нормализуем относительное отклонение от базового соотношения
        float t;
        if (current > baseRatio)
        {
            // Шире → от 0.5 до maxMatch
            t = (current - baseRatio) / (2f - baseRatio); // примерная верхняя граница
            t = Mathf.Clamp01(t);
            scaler.matchWidthOrHeight = Mathf.Lerp(0.5f, maxMatch, t);
        }
        else
        {
            // Уже → от 0.5 до minMatch
            t = (baseRatio - current) / baseRatio; // примерная нижняя граница
            t = Mathf.Clamp01(t);
            scaler.matchWidthOrHeight = Mathf.Lerp(0.5f, minMatch, t);
        }
    }
}
