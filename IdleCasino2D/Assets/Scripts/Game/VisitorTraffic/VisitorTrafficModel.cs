using System.Collections;
using UnityEngine;

public class VisitorTrafficModel
{
    private readonly ISpawnerVisitorProvider _spawnerProvider;
    private readonly ISpawnerVisitorInfoProvider _spawnerInfo;

    private IEnumerator trafficRoutine;

    private readonly float spawnInterval = 5f; // секунда между попытками спавна
    private readonly int maxVisitors = 10;

    public VisitorTrafficModel(ISpawnerVisitorProvider spawner, ISpawnerVisitorInfoProvider info)
    {
        _spawnerProvider = spawner;
        _spawnerInfo = info;
    }

    public void Initialize()
    {
        if (trafficRoutine != null)
            Coroutines.Stop(trafficRoutine);

        trafficRoutine = TrafficCoroutine();
        Coroutines.Start(trafficRoutine);
    }

    public void Dispose()
    {
        if (trafficRoutine != null)
            Coroutines.Stop(trafficRoutine);
    }

    public void PlayTraffic()
    {
        if (trafficRoutine != null)
            Coroutines.Stop(trafficRoutine);

        trafficRoutine = TrafficCoroutine();
        Coroutines.Start(trafficRoutine);
    }

    public void StopTraffic()
    {
        if (trafficRoutine != null)
            Coroutines.Stop(trafficRoutine);
    }

    private IEnumerator TrafficCoroutine()
    {
        while (true)
        {
            if (_spawnerInfo.CountVisitors < maxVisitors)
            {
                _spawnerProvider.SpawnVisitor();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
