using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProfitOfflineModel
{
    public bool IsActive => _managerStaff != null && _managerStaff.CountStaff > 0;
    public int Earn => _earn;

    private const string LAST_EXIT_TIME = "offline_exit_time";
    private const int PROFIT_INTERVAL = 120;         // 2 минуты
    private const int MAX_OFFLINE_PROFIT = 15000;

    private readonly List<ICasinoEntityInfo> _entities;
    private readonly ICasinoProfitStoreInfo _profitStore;
    private readonly ICasinoEntityStaff _managerStaff;
    private readonly IMoneyProvider _moneyProvider;

    private bool _profitCollected = true;
    private int _earn = 0;

    public ProfitOfflineModel(
        List<ICasinoEntityInfo> entities,
        ICasinoProfitStoreInfo profitStore,
        IMoneyProvider moneyProvider)
    {
        _entities = entities;
        _profitStore = profitStore;
        _moneyProvider = moneyProvider;

        // Берём первого менеджера, если есть
        _managerStaff = _entities
            .OfType<ICasinoEntityStaff>()
            .FirstOrDefault(data => data.PersonalType == StaffType.Hostess);
    }

    #region Lifecycle

    public void Initialize()
    {
        Debug.Log("ACTIVE EARN");
        Debug.Log(_managerStaff);
        Debug.Log(_managerStaff.CountStaff);

        if (!IsActive) return;

        Debug.Log("ACTIVE EARN");

        _profitCollected = false;

        if (!PlayerPrefs.HasKey(LAST_EXIT_TIME)) return;

        _earn = CalculateOfflineProfit();

        OnOfflineProfitCalculated?.Invoke(_earn, GetOfflineDurationText());
    }

    public void Dispose()
    {
        if (!_profitCollected) return;

        PlayerPrefs.SetString(
            LAST_EXIT_TIME,
            DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
        );
        PlayerPrefs.Save();
    }

    #endregion

    #region Public Methods

    public void CollectProfit()
    {
        Debug.Log("COLLECT");

        _profitCollected = true;

        if (_earn > 0)
            _moneyProvider.SendMoney(_earn);

        _earn = 0;

        OnCollectProfit?.Invoke();
    }

    #endregion

    #region Internal Logic

    public string GetOfflineDurationText()
    {
        if (!PlayerPrefs.HasKey(LAST_EXIT_TIME))
            return "0m"; // Если нет сохранённого времени

        long lastTime = long.Parse(PlayerPrefs.GetString(LAST_EXIT_TIME));
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        TimeSpan offlineSpan = TimeSpan.FromSeconds(now - lastTime);

        int days = offlineSpan.Days;
        int hours = offlineSpan.Hours;
        int minutes = offlineSpan.Minutes;

        string result = "";
        if (days > 0) result += $"{days}d ";
        if (hours > 0 || days > 0) result += $"{hours}h ";
        result += $"{minutes}m";

        return result.Trim();
    }

    private int CalculateOfflineProfit()
    {
        long lastTime = long.Parse(PlayerPrefs.GetString(LAST_EXIT_TIME));
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        int cycles = (int)((now - lastTime) / PROFIT_INTERVAL);
        if (cycles <= 0) return 0;

        int profit = 0;
        foreach (var entity in _entities)
        {
            if (!entity.IsOpen) continue;
            profit += _profitStore.GetProfit(entity.CasinoEntityType) * cycles;
        }

        return Mathf.Min(profit, MAX_OFFLINE_PROFIT);
    }

    #endregion

    #region Output Events

    public event Action<int, string> OnOfflineProfitCalculated;

    public event Action OnCollectProfit;

    #endregion
}
