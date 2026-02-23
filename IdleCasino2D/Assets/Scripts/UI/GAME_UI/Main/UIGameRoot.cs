using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameRoot : UIRoot
{
    [SerializeField] private StartPanel_Game startPanel;

    private ISoundProvider _soundProvider;

    public void SetSoundProvider(ISoundProvider soundProvider)
    {
        _soundProvider = soundProvider;
    }

    public void Initialize()
    {
        startPanel.Initialize();
    }

    public void Activate()
    {
        startPanel.OnClickToPlay += ClickToPlay_START;
    }

    public void Deactivate()
    {
        startPanel.OnClickToPlay -= ClickToPlay_START;

        if (currentPanel != null)
            CloseOtherPanel(currentPanel);
    }

    public void Dispose()
    {
        startPanel.Dispose();
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

    #endregion


    #region Output

    #region START

    public event Action OnClickToPlay_START;

    private void ClickToPlay_START()
    {
        OnClickToPlay_START?.Invoke();
    }

    #endregion

    #endregion
}
