using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogueModel
{
    private readonly Dictionary<string, string> words = new();

    public TutorialDialogueModel(DialogueMessagesSO dialogueMessages)
    {
        for (int i = 0; i < dialogueMessages.messages.Count; i++)
        {
            words.Add(dialogueMessages.messages[i].Id, dialogueMessages.messages[i].Message);
        }
    }

    public void SetMessage(string id, float duration)
    {
        if(words.TryGetValue(id, out string message))
        {
            OnSetMessage?.Invoke(message, duration);
        }
        else
        {
            Debug.Log("Not found TutorialMessage with ID - " + id);
        }
    }

    #region Output

    public event Action<string, float> OnSetMessage;

    #endregion
}
