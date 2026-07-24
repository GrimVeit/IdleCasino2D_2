using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogueModel
{
    private readonly Dictionary<string, string> words = new();

    private readonly ISoundProvider _soundProvider;

    public TutorialDialogueModel(DialogueMessagesSO dialogueMessages, ISoundProvider soundProvider)
    {
        for (int i = 0; i < dialogueMessages.messages.Count; i++)
        {
            words.Add(dialogueMessages.messages[i].Id, dialogueMessages.messages[i].Message);
        }

        _soundProvider = soundProvider;
    }

    public void SetMessage(string id, float duration)
    {
        if(words.TryGetValue(id, out string message))
        {
            OnSetMessage?.Invoke(message, duration);
            _soundProvider.PlayOneShot("Message");
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
