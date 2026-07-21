using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogueView : View
{
    [SerializeField] private MessageVisualModul messageVisualModul;

    public void SetMessage(string text, float duration)
    {
        messageVisualModul.SetMessage(text, SpeechTurnEnum.Right, duration);
    }
}
