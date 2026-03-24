using System;
using System.Collections.Generic;
using UnityEngine;

public class CasinoProfitModel
{
    private readonly ICasinoProfitStoreProvider _casinoProfitStoreProvider;
    private readonly ICasinoProfitStoreListener _casinoProfitStoreListener;
    private readonly ICasinoProfitStoreInfo _casinoProfitStoreInfo;
    private readonly IMoneyProvider _moneyProvider;
    private readonly ISoundProvider _soundProvider;

    private readonly Dictionary<CasinoEntityType, List<(int profitValue, int upgradeCost)>> _upgrades
        = new Dictionary<CasinoEntityType, List<(int, int)>>();

    private CasinoEntityType _currentSelectedType;

    public CasinoProfitModel(ICasinoProfitStoreProvider casinoProfitStoreProvider, ICasinoProfitStoreListener casinoProfitStoreListener, ICasinoProfitStoreInfo casinoProfitStoreInfo, IMoneyProvider moneyProvider, ISoundProvider soundProvider)
    {
        _casinoProfitStoreProvider = casinoProfitStoreProvider;
        _casinoProfitStoreListener = casinoProfitStoreListener;
        _casinoProfitStoreInfo = casinoProfitStoreInfo;
        _moneyProvider = moneyProvider;
        _soundProvider = soundProvider;

        _upgrades[CasinoEntityType.Bar] = new List<(int, int)> { (10, 0), (15, 100), (20, 200), (28, 350), (35, 500), (45, 700), (55, 950), (70, 1300), (85, 1700), (100, 2000)  };
        _upgrades[CasinoEntityType.Music] = new List<(int, int)> { (10, 0), (15, 100), (20, 200), (28, 350), (35, 500), (45, 700), (55, 950), (70, 1300), (85, 1700), (100, 2000) };
        _upgrades[CasinoEntityType.Poker] = new List<(int, int)> { (10, 0), (15, 100), (20, 200), (28, 350), (35, 500), (45, 700), (55, 950), (70, 1300), (85, 1700), (100, 2000) };
        _upgrades[CasinoEntityType.Roulette] = new List<(int, int)> { (10, 0), (15, 100), (20, 200), (28, 350), (35, 500), (45, 700), (55, 950), (70, 1300), (85, 1700), (100, 2000) };
        _upgrades[CasinoEntityType.Slot] = new List<(int, int)> { (10, 0), (15, 100), (20, 200), (28, 350), (35, 500), (45, 700), (55, 950), (70, 1300), (85, 1700), (100, 2000) };
        _upgrades[CasinoEntityType.Wheel] = new List<(int, int)> { (10, 0), (15, 100), (20, 200), (28, 350), (35, 500), (45, 700), (55, 950), (70, 1300), (85, 1700), (100, 2000) };

    }

    public void Initialize()
    {
        _casinoProfitStoreListener.OnProfitStoreChanged += OnProfitStoreChanged;

        foreach (CasinoEntityType type in Enum.GetValues(typeof(CasinoEntityType)))
        {
            int profit = _casinoProfitStoreInfo.GetProfit(type);
            UpdateLevel(type, profit);
        }
    }

    public void Dispose()
    {

    }

    private void OnProfitStoreChanged(CasinoEntityType type, int value)
    {
        UpdateLevel(type, value);
    }

    private void UpdateLevel(CasinoEntityType type, int profitValue)
    {
        if (!_upgrades.ContainsKey(type)) return;

        var list = _upgrades[type];
        int index = list.FindIndex(x => x.profitValue == profitValue);
        if (index < 0) index = 0;

        var nextLevel = index < list.Count - 1 ? list[index + 1] : (0, 0);

        if (_currentSelectedType == type)
        {
            var currentLevel = list[index];

            OnUpdateDetailPanel?.Invoke(
                type,
                currentLevel.profitValue,
                index + 1,
                list.Count,
                nextLevel.Item2
            );
        }

        OnUpdateMain?.Invoke(type, nextLevel.Item2);
    }

    // выбор типа казино через View
    public void SetCasinoType(CasinoEntityType casinoEntityType)
    {
        _currentSelectedType = casinoEntityType;
        OnChooseEntityType?.Invoke(casinoEntityType);

        // сразу обновляем детальную панель
        int profit = _casinoProfitStoreInfo.GetProfit(casinoEntityType);
        UpdateLevel(casinoEntityType, profit);
    }

    // апгрейд текущего выбранного типа
    public void UpgradeCurrentType()
    {
        if (_currentSelectedType == default || !_upgrades.ContainsKey(_currentSelectedType)) return;

        var list = _upgrades[_currentSelectedType];
        int currentIndex = list.FindIndex(x => x.profitValue == _casinoProfitStoreInfo.GetProfit(_currentSelectedType));
        if (currentIndex < 0) currentIndex = 0;
        if (currentIndex >= list.Count - 1) return;

        var cost = list[currentIndex + 1].upgradeCost;

        if (_moneyProvider.CanAfford(cost))
        {
            _moneyProvider.SendMoney(-cost);
            int newProfit = list[currentIndex + 1].profitValue;
            _casinoProfitStoreProvider.SetProfit(_currentSelectedType, newProfit);
        }
        else
        {
            Debug.Log("ERROR");
            _soundProvider.PlayOneShot("Error");
        }
    }

    #region Output

    public event Action<CasinoEntityType> OnChooseEntityType;
    public event Action<CasinoEntityType, int, int, int, int> OnUpdateDetailPanel;
    public event Action<CasinoEntityType, int> OnUpdateMain;

    #endregion
}

[Serializable]
public class UpgradeLevel
{
    public int ProfitValue;
    public int UpgradeCost;
}

[Serializable]
public class CasinoTypeUpgrades
{
    public CasinoEntityType Type;
    public List<UpgradeLevel> Levels = new();
    public int CurrentLevelIndex;

    public UpgradeLevel CurrentLevel => Levels[Mathf.Clamp(CurrentLevelIndex, 0, Levels.Count - 1)];
    public UpgradeLevel NextLevel => (CurrentLevelIndex < Levels.Count - 1) ? Levels[CurrentLevelIndex + 1] : null;
}
