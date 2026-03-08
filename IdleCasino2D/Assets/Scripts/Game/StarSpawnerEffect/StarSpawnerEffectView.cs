using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawnerEffectView : View
{
    public StarEffect starPrefab;
    public Transform[] spawnPoints; // 200 точек
    public int poolSize = 50;       // сколько звёзд держим одновременно

    private List<StarEffect> starPool = new List<StarEffect>();

    void Start()
    {
        // Создаём пул
        for (int i = 0; i < poolSize; i++)
        {
            StarEffect star = Instantiate(starPrefab);
            starPool.Add(star);
        }

        // Начинаем спавн
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator<WaitForSeconds> SpawnLoop()
    {
        while (true)
        {
            // Случайный интервал между спавнами
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));

            // Находим свободную звезду
            StarEffect freeStar = starPool.Find(s => !s.IsWork);
            if (freeStar != null)
            {
                // Рандомная точка
                Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
                freeStar.transform.position = point.position;

                // Активируем
                freeStar.Activate();
            }
        }
    }
}
