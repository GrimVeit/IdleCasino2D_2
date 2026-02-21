using System;
using UnityEngine;

public class UIMainMenuRoot : UIRoot
{
    private ISoundProvider _soundProvider;

    public void SetSoundProvider(ISoundProvider soundProvider)
    {
        _soundProvider = soundProvider;
    }

    public void Initialize()
    {
        
    }

    public void Activate()
    {
        
    }


    public void Deactivate()
    {
        
        if (currentPanel != null)
            CloseOtherPanel(currentPanel);
    }

    public void Dispose()
    {
        
    }

    #region INPUT


    #endregion


    #region OUTPUT


    #endregion

}
