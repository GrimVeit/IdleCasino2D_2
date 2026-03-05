using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CasinoProfitStoreModel
{
    private readonly Dictionary<CasinoEntityType, int> _profits = new();

    private const string PlayerPrefsKey = "CasinoProfits";

    public void Initialize()
    {
        _profits.Clear();

        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            string json = PlayerPrefs.GetString(PlayerPrefsKey);
            var savedDict = JsonUtility.FromJson<SerializableDictionary>(json);
            foreach (var kv in savedDict.items)
            {
                if (Enum.TryParse<CasinoEntityType>(kv.key, out var type))
                    _profits[type] = kv.value;
            }
        }
        else
        {
            foreach (CasinoEntityType type in Enum.GetValues(typeof(CasinoEntityType)))
                _profits[type] = 10;
        }

        for (int i = 0; i < _profits.Count; i++)
        {
            var element = _profits.ElementAt(i);
            OnChangeProfitValue?.Invoke(element.Key, element.Value);
            Debug.Log($"CASINO TYPE - {element.Key}, PROFIT VALUE - {element.Value}");
        }
    }

    public void Dispose()
    {
        var serializableDict = new SerializableDictionary();
        foreach (var kv in _profits)
        {
            serializableDict.items.Add(new SerializableDictionary.Item { key = kv.Key.ToString(), value = kv.Value });
        }

        string json = JsonUtility.ToJson(serializableDict);
        PlayerPrefs.SetString(PlayerPrefsKey, json);
        PlayerPrefs.Save();
    }

    public void SetProfit(CasinoEntityType type, int value)
    {
        _profits[type] = value;

        OnChangeProfitValue?.Invoke(type, value);
    }

    public int GetProfit(CasinoEntityType type)
    {
        return _profits.TryGetValue(type, out int value) ? value : 10;
    }

    [Serializable]
    private class SerializableDictionary
    {
        [Serializable]
        public class Item
        {
            public string key;
            public int value;
        }

        public List<Item> items = new();
    }

    #region Output

    public event Action<CasinoEntityType, int> OnChangeProfitValue;

    #endregion
}
