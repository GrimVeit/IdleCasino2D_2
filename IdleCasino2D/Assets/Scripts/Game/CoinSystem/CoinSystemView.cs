using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSystemView : View
{
    [SerializeField] private Coin coinPrefab;
    [SerializeField] private Transform coinSpawnParent;

    public List<Coin> _activeCoins = new();
    public void AddCoin(Vector3 position, int count)
    {
        var coin = Instantiate(coinPrefab, coinSpawnParent);
        coin.transform.SetPositionAndRotation(position, Quaternion.identity);
        coin.SetData(count);

        coin.OnCollectCoin += HandleCoinCollected;
        coin.Initialize();

        _activeCoins.Add(coin);  // добавляем в список

        coin.PlaySpawnAnimation();
    }

    private void HandleCoinCollected(Coin coin, int value)
    {
        coin.OnCollectCoin -= HandleCoinCollected;

        OnCoinCollected?.Invoke(value);

        if (_activeCoins.Contains(coin))
            _activeCoins.Remove(coin);

        coin.PlayCollectDestroyAnimation();
        coin.Dispose();
    }

    public void RemoveAllCoins()
    {
        foreach (var coin in _activeCoins)
        {
            if (coin != null)
            {
                coin.PlayCollectDestroyAnimation();
                coin.Dispose();
            }
        }
        _activeCoins.Clear();
    }

    #region Output

    public event Action<int> OnCoinCollected; // событие для передачи количества монеток при сборе

    #endregion
}
