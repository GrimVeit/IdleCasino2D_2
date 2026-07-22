using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueMessagesSO", menuName = "Game/Dialogue/Messages")]
public class DialogueMessagesSO : ScriptableObject
{
    public List<DialogueMessage> messages = new();

    [Serializable]
    public class DialogueMessage
    {
        public string Id => id;
        public string Message => message;

        [SerializeField] private string id;
        [SerializeField] private string message;
    }
}
