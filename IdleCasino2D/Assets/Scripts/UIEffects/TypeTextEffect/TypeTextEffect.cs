using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeTextEffect : UIEffect
{
    public string Text => typeText.TextDescription;

    [SerializeField] private TypeText typeText;
    [SerializeField] private float speedType;

    private IEnumerator typingText;
    private IEnumerator cleaningText;

    public void SetText(string text)
    {
        typeText.SetTextDescription(text);
    }

    public override void ActivateEffect(Action OnComplete = null)
    {
        ClearAllCoroutines();
        isActive = true;
        typingText = DisplayTypingText();
        Coroutines.Start(typingText);
    }

    public override void DeactivateEffect(Action OnComplete = null)
    {
        ClearAllCoroutines();
        isActive = false;
        //Debug.Log("Activate clean");

        cleaningText = DisplayCleaningText();
        Coroutines.Start(cleaningText);
    }

    public override void ResetEffect()
    {
        ClearAllCoroutines();

        typeText.TextComponent.text = "";
    }

    public void ClearAllCoroutines()
    {
        if (typingText != null)
            Coroutines.Stop(typingText);
        if (cleaningText != null)
            Coroutines.Stop(cleaningText);
    }

    private IEnumerator DisplayTypingText()
    {
        yield return StartTyping(typeText.TextComponent, typeText.TextDescription, typeText.TimePause);
    }

    private IEnumerator DisplayCleaningText()
    {
        yield return StartCleaning(typeText.TextComponent, typeText.TextDescription, typeText.TimePause);
    }

    private IEnumerator StartTyping(TextMeshProUGUI textComponent, string text, float timePause)
    {
        textComponent.text = "";

        foreach (char letter in text)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(speedType);
        }

        yield return new WaitForSeconds(timePause);
    }

    private IEnumerator StartCleaning(TextMeshProUGUI textComponent, string text, float timePause)
    {
        textComponent.text = text;

        //Debug.Log(textComponent.text);

        for (int i = text.Length; i >= 0; i--)
        {
            textComponent.text = text[..i];
            yield return new WaitForSeconds(speedType);
        }

        yield return new WaitForSeconds(timePause);
    }
}
