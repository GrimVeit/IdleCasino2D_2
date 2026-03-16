using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProfitOfflineModel
{
    public bool IsActive => _managerStaff != null && _managerStaff.CountStaff > 0;
    public int Earn => _earn;

    private const string LAST_EXIT_TIME = "offline_exit_time";
    private const int PROFIT_INTERVAL = 120;
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

        _managerStaff = _entities
            .OfType<ICasinoEntityStaff>()
            .FirstOrDefault(data => data.PersonalType == StaffType.Hostess);
    }

    #region Lifecycle

    public void Initialize()
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        if (!PlayerPrefs.HasKey(LAST_EXIT_TIME))
        {
            PlayerPrefs.SetString(LAST_EXIT_TIME, now.ToString());
            return;
        }

        long lastTime = long.Parse(PlayerPrefs.GetString(LAST_EXIT_TIME));

        // защита от перемотки времени назад
        if (lastTime > now)
        {
            PlayerPrefs.SetString(LAST_EXIT_TIME, now.ToString());
            return;
        }

        if (!IsActive)
            return;

        _profitCollected = false;

        _earn = CalculateOfflineProfit(lastTime, now);

        if (_earn > 0)
        {
            OnOfflineProfitCalculated?.Invoke(_earn, GetOfflineDurationText(lastTime, now));
        }
    }

    public void Dispose()
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        if (!PlayerPrefs.HasKey(LAST_EXIT_TIME))
        {
            PlayerPrefs.SetString(LAST_EXIT_TIME, now.ToString());
            return;
        }

        long lastTime = long.Parse(PlayerPrefs.GetString(LAST_EXIT_TIME));

        // защита от перемотки времени назад
        if (lastTime > now)
        {
            PlayerPrefs.SetString(LAST_EXIT_TIME, now.ToString());
            return;
        }

        // если прибыль была рассчитана и НЕ забрана — не обновляем время
        if (!_profitCollected && _earn > 0)
            return;

        PlayerPrefs.SetString(LAST_EXIT_TIME, now.ToString());
    }

    #endregion

    #region Public

    public void CollectProfit()
    {
        _profitCollected = true;

        if (_earn > 0)
            _moneyProvider.SendMoney(_earn);

        _earn = 0;

        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        PlayerPrefs.SetString(LAST_EXIT_TIME, now.ToString());

        OnCollectProfit?.Invoke();
    }

    #endregion

    #region Internal

    private int CalculateOfflineProfit(long lastTime, long now)
    {
        int cycles = (int)((now - lastTime) / PROFIT_INTERVAL);

        if (cycles <= 0)
            return 0;

        int profit = 0;

        foreach (var entity in _entities)
        {
            if (!entity.IsOpen)
                continue;

            profit += _profitStore.GetProfit(entity.CasinoEntityType) * cycles;
        }

        return Mathf.Min(profit, MAX_OFFLINE_PROFIT);
    }

    public string GetOfflineDurationText(long lastTime, long now)
    {
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

    #endregion

    #region Events

    public event Action<int, string> OnOfflineProfitCalculated;
    public event Action OnCollectProfit;

    #endregion
}
