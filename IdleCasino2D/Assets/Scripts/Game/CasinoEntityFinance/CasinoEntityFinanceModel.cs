using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoEntityFinanceModel
{
    private readonly List<ICasinoEntityProfit> _casinoEntityProfits = new();
    private readonly ICoinSystemProvider _coinSystemProvider;

    public CasinoEntityFinanceModel(List<ICasinoEntityInfo> casinoEntityInfos, ICoinSystemProvider coinSystemProvider)
    {
        foreach (var entity in casinoEntityInfos)
        {
            if (entity is ICasinoEntityProfit profit)
            {
                _casinoEntityProfits.Add(profit);
            }
        }

        _coinSystemProvider = coinSystemProvider;

        for (int i = 0; i < _casinoEntityProfits.Count; i++)
        {
            _casinoEntityProfits[i].OnAddCoins += AddCoin;
        }
    }

    public void Dispose()
    {
        for (int i = 0; i < _casinoEntityProfits.Count; i++)
        {
            _casinoEntityProfits[i].OnAddCoins -= AddCoin;
        }
    }

    private void AddCoin(Vector3 position, int amount)
    {
        _coinSystemProvider.AddCoin(position, amount);
    }
}
