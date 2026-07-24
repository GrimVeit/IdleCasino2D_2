using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProfitOfflineModel : IDisposable
{
    public bool IsActive =>
        _managerStaff != null && _managerStaff.CountStaff > 0;

    public int Earn => _earn;


    private const string LAST_EXIT_TIME = "offline_exit_time";


    // Баланс офлайн дохода
    private const int OFFLINE_PROFIT_PER_DAY = 1000;
    private const int MAX_OFFLINE_PROFIT = 15000;


    private readonly List<ICasinoEntityInfo> _entities;
    private readonly ICasinoProfitStoreInfo _profitStore;
    private readonly ICasinoEntityStaff _managerStaff;
    private readonly IMoneyProvider _moneyProvider;


    private bool _profitCollected = true;
    private int _earn;


    public event Action<int, string> OnOfflineProfitCalculated;
    public event Action OnCollectProfit;


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
            .FirstOrDefault(data =>
                data.PersonalType == StaffType.Hostess);
    }


    #region Lifecycle


    public void Initialize()
    {
        long now = GetCurrentTime();


        if (!PlayerPrefs.HasKey(LAST_EXIT_TIME))
        {
            SaveExitTime(now);
            return;
        }


        long lastTime = GetLastExitTime();


        // защита от перемотки времени
        if (lastTime > now)
        {
            SaveExitTime(now);
            return;
        }


        if (!IsActive)
            return;


        _profitCollected = false;


        _earn = CalculateOfflineProfit(
            lastTime,
            now
        );


        if (_earn > 0)
        {
            OnOfflineProfitCalculated?.Invoke(
                _earn,
                GetOfflineDurationText(
                    lastTime,
                    now
                )
            );
        }
    }



    public void Dispose()
    {
        SaveExitTime();
    }



    private void SaveExitTime()
    {
        long now = GetCurrentTime();

        // не двигаем время,
        // пока игрок не забрал награду
        if (!_profitCollected && _earn > 0)
            return;


        SaveExitTime(now);
    }



    private void SaveExitTime(long time)
    {
        PlayerPrefs.SetString(
            LAST_EXIT_TIME,
            time.ToString()
        );

        PlayerPrefs.Save();
    }


    #endregion



    #region Public


    public void CollectProfit()
    {
        _profitCollected = true;


        if (_earn > 0)
        {
            _moneyProvider.SendMoney(_earn);
        }


        _earn = 0;


        SaveExitTime();


        OnCollectProfit?.Invoke();
    }


    #endregion



    #region Calculation



    private int CalculateOfflineProfit(
        long lastTime,
        long now)
    {
        long offlineSeconds =
            now - lastTime;


        if (offlineSeconds <= 0)
            return 0;



        double days =
            offlineSeconds /
            (60d * 60d * 24d);



        float multiplier =
            GetOfflineMultiplier();



        int profit =
            Mathf.FloorToInt(
                (float)(
                    days *
                    OFFLINE_PROFIT_PER_DAY *
                    multiplier
                )
            );



        return Mathf.Min(
            profit,
            MAX_OFFLINE_PROFIT
        );
    }



    private float GetOfflineMultiplier()
    {
        int openedTables =
            _entities.Count(entity =>
                entity.IsOpen
            );


        /*
         * Пример:
         *
         * 0 столов   = x1
         * 50 столов  = x1.5
         * 100 столов = x2
         * 200+      = x3
         */


        return Mathf.Clamp(
            1f + openedTables * 0.01f,
            1f,
            3f
        );
    }



    #endregion



    #region Time



    private long GetCurrentTime()
    {
        return DateTimeOffset
            .UtcNow
            .ToUnixTimeSeconds();
    }



    private long GetLastExitTime()
    {
        string value =
            PlayerPrefs.GetString(
                LAST_EXIT_TIME
            );


        if (long.TryParse(
                value,
                out long result))
        {
            return result;
        }


        return GetCurrentTime();
    }



    #endregion



    #region UI



    public string GetOfflineDurationText(
        long lastTime,
        long now)
    {
        TimeSpan offlineSpan =
            TimeSpan.FromSeconds(
                now - lastTime
            );


        int days = offlineSpan.Days;
        int hours = offlineSpan.Hours;
        int minutes = offlineSpan.Minutes;


        string result = "";


        if (days > 0)
            result += $"{days}d ";


        if (hours > 0 || days > 0)
            result += $"{hours}h ";


        result += $"{minutes}m";


        return result.Trim();
    }


    #endregion
}
