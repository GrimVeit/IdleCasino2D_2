using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameRoot : UIRoot
{
    [SerializeField] private StartPanel_Game startPanel;
    [SerializeField] private MainPanel_Game mainPanel;
    [SerializeField] private AvatarBalancePanel_Game avatarBalancePanel;
    [SerializeField] private BlackBackgroundPanel_Game blackBackgroundPanel;
    [SerializeField] private UpgradePanel_Game upgradePanel;

    [SerializeField] private HireStaffPanel_Game hireStaffPanel;
    [SerializeField] private SelectStaffPanel_Game selectStaffPanel;

    [SerializeField] private ShopSpotPanel_Game shopSpotPanel;

    [SerializeField] private ChooseAvailableSpotPanel_Game chooseAvailableSpotPanel;
    [SerializeField] private LeavePanel_Game leavePanel;

    private ISoundProvider _soundProvider;

    public void SetSoundProvider(ISoundProvider soundProvider)
    {
        _soundProvider = soundProvider;
    }

    public void Initialize()
    {
        startPanel.Initialize();
        mainPanel.Initialize();
        avatarBalancePanel.Initialize();
        blackBackgroundPanel.Initialize();
        upgradePanel.Initialize();

        hireStaffPanel.Initialize();
        selectStaffPanel.Initialize();

        shopSpotPanel.Initialize();

        chooseAvailableSpotPanel.Initialize();
        leavePanel.Initialize();
    }

    public void Activate()
    {
        startPanel.OnClickToPlay += ClickToPlay_START;

        mainPanel.OnClickToUpgrade += ClickToUpgrade_MAIN;
        mainPanel.OnClickToHireStaff += ClickToHireStaff_MAIN;

        upgradePanel.OnClickToBack += ClickToBack_UPGRADE;
        hireStaffPanel.OnClickToBack += ClickToBack_HIRE_STAFF;
        selectStaffPanel.OnClickToBack += ClickToBack_SELECT_STAFF;
        shopSpotPanel.OnClickToBack += ClickToBack_SHOP_SPOT;
    }

    public void Deactivate()
    {
        startPanel.OnClickToPlay -= ClickToPlay_START;

        mainPanel.OnClickToUpgrade -= ClickToUpgrade_MAIN;
        mainPanel.OnClickToHireStaff -= ClickToHireStaff_MAIN;

        upgradePanel.OnClickToBack -= ClickToBack_UPGRADE;
        hireStaffPanel.OnClickToBack -= ClickToBack_HIRE_STAFF;
        selectStaffPanel.OnClickToBack -= ClickToBack_SELECT_STAFF;
        shopSpotPanel.OnClickToBack -= ClickToBack_SHOP_SPOT;

        if (currentPanel != null)
            CloseOtherPanel(currentPanel);
    }

    public void Dispose()
    {
        startPanel.Dispose();
        mainPanel.Dispose();
        avatarBalancePanel.Dispose();
        blackBackgroundPanel.Dispose();
        upgradePanel.Dispose();

        hireStaffPanel.Dispose();
        selectStaffPanel.Dispose();

        shopSpotPanel.Dispose();

        chooseAvailableSpotPanel.Dispose();
        leavePanel.Dispose();
    }

    #region Input

    public void OpenStartPanel()
    {
        if(startPanel.IsActive) return;

        OpenOtherPanel(startPanel);
    }

    public void CloseStartPanel()
    {
        if (!startPanel.IsActive) return;

        CloseOtherPanel(startPanel);
    }



    public void OpenMainPanel()
    {
        if (mainPanel.IsActive) return;

        OpenOtherPanel(mainPanel);
    }

    public void CloseMainPanel()
    {
        if (!mainPanel.IsActive) return;

        CloseOtherPanel(mainPanel);
    }



    public void OpenAvatarBalancePanel()
    {
        if (avatarBalancePanel.IsActive) return;

        OpenOtherPanel(avatarBalancePanel);
    }

    public void CloseAvatarBalancePanel()
    {
        if (!avatarBalancePanel.IsActive) return;

        CloseOtherPanel(avatarBalancePanel);
    }



    public void OpenBlackBackgroundPanel()
    {
        if (blackBackgroundPanel.IsActive) return;

        OpenOtherPanel(blackBackgroundPanel);
    }

    public void CloseBlackBackgroundPanel()
    {
        if (!blackBackgroundPanel.IsActive) return;

        CloseOtherPanel(blackBackgroundPanel);
    }




    public void OpenUpgradePanel()
    {
        if (upgradePanel.IsActive) return;

        OpenOtherPanel(upgradePanel);
    }

    public void CloseUpgradePanel()
    {
        if (!upgradePanel.IsActive) return;

        CloseOtherPanel(upgradePanel);
    }




    public void OpenHireStaffPanel()
    {
        if (hireStaffPanel.IsActive) return;

        OpenOtherPanel(hireStaffPanel);
    }

    public void CloseHireStaffPanel()
    {
        if (!hireStaffPanel.IsActive) return;

        CloseOtherPanel(hireStaffPanel);
    }



    public void OpenSelectStaffPanel()
    {
        if (selectStaffPanel.IsActive) return;

        OpenOtherPanel(selectStaffPanel);
    }

    public void CloseSelectStaffPanel()
    {
        if (!selectStaffPanel.IsActive) return;

        CloseOtherPanel(selectStaffPanel);
    }






    public void OpenShopSpotPanel()
    {
        if (shopSpotPanel.IsActive) return;

        OpenOtherPanel(shopSpotPanel);
    }

    public void CloseShopSpotPanel()
    {
        if (!shopSpotPanel.IsActive) return;

        CloseOtherPanel(shopSpotPanel);
    }





    public void OpenChooseAvailableSpotPanel()
    {
        if(chooseAvailableSpotPanel.IsActive) return;

        OpenOtherPanel(chooseAvailableSpotPanel);
    }

    public void CloseChooseAvailableSpotPanel()
    {
        if(!chooseAvailableSpotPanel.IsActive) return;

        CloseOtherPanel(chooseAvailableSpotPanel);
    }


    public void OpenLeavePanel()
    {
        if (leavePanel.IsActive) return;

        OpenOtherPanel(leavePanel);
    }

    public void CloseLeavePanel()
    {
        if (!leavePanel.IsActive) return;

        CloseOtherPanel(leavePanel);
    }

    #endregion


    #region Output

    #region START

    public event Action OnClickToPlay_START;

    private void ClickToPlay_START()
    {
        OnClickToPlay_START?.Invoke();
    }

    #endregion

    #region MAIN

    public event Action OnCLickToUpgrade_MAIN;
    public event Action OnClickToHireStaff_MAIN;

    private void ClickToUpgrade_MAIN()
    {
        OnCLickToUpgrade_MAIN?.Invoke();
    }

    private void ClickToHireStaff_MAIN()
    {
        OnClickToHireStaff_MAIN?.Invoke();
    }

    #endregion

    #region UPGRADE

    public event Action OnClickToBack_UPGRADE;

    private void ClickToBack_UPGRADE()
    {
        OnClickToBack_UPGRADE?.Invoke();
    }

    #endregion

    #region HIRE STAFF

    public event Action OnClickToBack_HIRE_STAFF;

    private void ClickToBack_HIRE_STAFF()
    {
        OnClickToBack_HIRE_STAFF?.Invoke();
    }

    #endregion

    #region SELECT STAFF

    public event Action OnClickToBack_SELECT_STAFF;

    private void ClickToBack_SELECT_STAFF()
    {
        OnClickToBack_SELECT_STAFF?.Invoke();
    }

    #endregion

    #region SHOP SPOT

    public event Action OnClickToBack_SHOP_SPOT;

    private void ClickToBack_SHOP_SPOT()
    {
        OnClickToBack_SHOP_SPOT?.Invoke();
    }

    #endregion

    #endregion
}
