using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorCounterTrafficModel
{
    private readonly ISpawnerVisitorProvider _spawnerProvider;
    private readonly ISpawnerVisitorInfoProvider _spawnerInfo;
    private readonly ICasinoEntityInfo _casinoEntityInfo;

    private IEnumerator trafficRoutine;

    private readonly float spawnInterval = 3f; // секунда между попытками спавна
    private readonly int maxVisitors = 15;

    public VisitorCounterTrafficModel(ISpawnerVisitorProvider spawner, ISpawnerVisitorInfoProvider info, List<ICasinoEntityInfo> casinoEntityInfos)
    {
        _spawnerProvider = spawner;
        _spawnerInfo = info;
        _casinoEntityInfo = casinoEntityInfos.Find(info => info.CasinoEntityType == CasinoEntityType.EntranceQueue);
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
            if (_spawnerInfo.CountVisitors < maxVisitors && _casinoEntityInfo.CanJoin)
            {
                _spawnerProvider.SpawnVisitor();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
