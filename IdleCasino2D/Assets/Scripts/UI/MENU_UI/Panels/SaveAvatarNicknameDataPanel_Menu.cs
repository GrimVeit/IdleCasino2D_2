using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveAvatarNicknameDataPanel_Menu : MovePanel
{
    [SerializeField] private Button buttonSave;

    public override void Initialize()
    {
        base.Initialize();

        buttonSave.onClick.AddListener(() => OnClickToSave?.Invoke());
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonSave.onClick.RemoveListener(() => OnClickToSave?.Invoke());
    }

    #region Output

    public event Action OnClickToSave;

    #endregion
}
