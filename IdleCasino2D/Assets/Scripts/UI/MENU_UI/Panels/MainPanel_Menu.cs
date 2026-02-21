using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel_Menu : MovePanel
{
    [SerializeField] private Button buttonLeaderboard;
    [SerializeField] private Button buttonRules;
    [SerializeField] private Button buttonProfile;
    [SerializeField] private Button buttonBalance;
    [SerializeField] private Button buttonSettings;
    [SerializeField] private Button buttonShop;
    [SerializeField] private Button buttonNewGame;

    [SerializeField] private List<UIEffectCombination> uIEffectCombinations = new List<UIEffectCombination>();

    public override void Initialize()
    {
        base.Initialize();

        buttonLeaderboard.onClick.AddListener(() => OnClickToLeaderboard?.Invoke());
        buttonRules.onClick.AddListener(() => OnClickToRules?.Invoke());
        buttonBalance.onClick.AddListener(() => OnClickToBalance?.Invoke());
        buttonProfile.onClick.AddListener(() => OnClickToProfile?.Invoke());
        buttonSettings.onClick.AddListener(() => OnClickToSettings?.Invoke());
        buttonShop.onClick.AddListener(() => OnClickToShop?.Invoke());
        buttonNewGame.onClick.AddListener(() => OnClickToNewGame?.Invoke());

        uIEffectCombinations.ForEach(data => data.Initialize());
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonLeaderboard.onClick.RemoveListener(() => OnClickToLeaderboard?.Invoke());
        buttonRules.onClick.RemoveListener(() => OnClickToRules?.Invoke());
        buttonBalance.onClick.RemoveListener(() => OnClickToBalance?.Invoke());
        buttonProfile.onClick.RemoveListener(() => OnClickToProfile?.Invoke());
        buttonSettings.onClick.RemoveListener(() => OnClickToSettings?.Invoke());
        buttonShop.onClick.RemoveListener(() => OnClickToShop?.Invoke());
        buttonNewGame.onClick.RemoveListener(() => OnClickToNewGame?.Invoke());

        uIEffectCombinations.ForEach(data => data.Dispose());
    }

    public override void ActivatePanel()
    {
        base.ActivatePanel();

        uIEffectCombinations.ForEach(data => data.ActivateEffect());
    }

    public override void DeactivatePanel()
    {
        base.DeactivatePanel();

        uIEffectCombinations.ForEach(data => data.DeactivateEffect());
    }

    #region Output

    public event Action OnClickToLeaderboard;
    public event Action OnClickToRules;
    public event Action OnClickToBalance;
    public event Action OnClickToProfile;
    public event Action OnClickToSettings;
    public event Action OnClickToShop;
    public event Action OnClickToNewGame;

    #endregion
}
