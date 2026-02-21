using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coroutines : MonoBehaviour
{
    private static Coroutines instance
    {
        get
        {
            if (m_instance == null)
            {
                var go = new GameObject("[COROUTINE MANAGER]");
                m_instance = go.AddComponent<Coroutines>();
                DontDestroyOnLoad(go);
            }
            return m_instance;
        }
    }

    private static Coroutines m_instance;

    public static void Start(IEnumerator enumerator)
    {
        instance.StartCoroutine(enumerator);
    }

    public static void Stop(IEnumerator enumerator)
    {
        instance.StopCoroutine(enumerator);
    }
}
