using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoEntityStoreModel
{
    private const string PlayerPrefsKey = "OpenCasinoEntityIndexes";
    private readonly List<ICasinoEntityInfo> _entities;

    public CasinoEntityStoreModel(List<ICasinoEntityInfo> entities)
    {
        _entities = entities;
    }

    public void Initialize()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsKey)) return;

        string json = PlayerPrefs.GetString(PlayerPrefsKey);
        var indexes = JsonUtility.FromJson<IntArrayWrapper>(json).Values;

        for (int i = 0; i < indexes.Length; i++)
        {
            int idx = indexes[i];
            if (idx >= 0 && idx < _entities.Count)
            {
                if (_entities[idx] is ICasinoEntityActivator activator)
                    activator.Open();
            }
        }
    }

    public void Dispose()
    {
        List<int> openIndexes = new List<int>();

        for (int i = 0; i < _entities.Count; i++)
        {
            if (_entities[i].IsOpen)
                openIndexes.Add(i);
        }

        var wrapper = new IntArrayWrapper() { Values = openIndexes.ToArray() };
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(PlayerPrefsKey, json);
        PlayerPrefs.Save();
    }

    [System.Serializable]
    private class IntArrayWrapper
    {
        public int[] Values;
    }
}
