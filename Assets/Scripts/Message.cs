using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

    [System.Serializable]
    public class Message{
        public string text;
        public TextMeshProUGUI tmp;
        public MessageType messageType;

        public enum MessageType
        {
            PlayerMessage,
            Info,
        }
    }

