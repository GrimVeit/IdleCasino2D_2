using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NicknameView : View
{
    public event Action OnSubmitNickname;
    public event Action<string> OnChangeNickname;

    [SerializeField] private List<TextMeshProUGUI> textNicknames = new();
    [SerializeField] private List<TMP_InputField> textInputs = new();

    [SerializeField] private List<TMP_InputField> inputFieldNickname;
    [SerializeField] private List<TypeTextEffect> typeTextEffects;

    [SerializeField] private Button buttonSave;
    [SerializeField] private UIEffect buttonEffect;

    public void Initialize()
    {
        for (int i = 0; i < inputFieldNickname.Count; i++)
        {
            inputFieldNickname[i].onValueChanged.AddListener(HandlerOnNicknameTextValueChanged);
        }

        if(buttonEffect != null)
           buttonEffect.Initialize();
    }

    public void Dispose()
    {
        for (int i = 0; i < inputFieldNickname.Count; i++)
        {
            inputFieldNickname[i].onValueChanged.RemoveListener(HandlerOnNicknameTextValueChanged);
        }

        if (buttonEffect != null)
            buttonEffect.Dispose();
    }

    public void ActivateButton()
    {
        buttonSave.enabled = true;

        buttonEffect.ActivateEffect();
    }

    public void DeactivateButton()
    {
        buttonSave.enabled = false;

        buttonEffect.DeactivateEffect();
    }

    public void ChangeNickname(string nickname)
    {
        for (int i = 0; i < textNicknames.Count; i++)
        {
            textNicknames[i].text = nickname;
        }

        for (int i = 0; i < textInputs.Count; i++)
        {
            textInputs[i].text = nickname;
        }
    }

    public void DisplayDescription(string text)
    {
        for (int i = 0; i < typeTextEffects.Count; i++)
        {
            if (typeTextEffects[i].Text != text)
            {
                typeTextEffects[i].ResetEffect();
                typeTextEffects[i].SetText(text);
                typeTextEffects[i].ActivateEffect();
            }
        }
    }

    #region Input

    private void HandlerOnNicknameTextValueChanged(string value)
    {
        OnChangeNickname?.Invoke(value);
    }

    #endregion
}
