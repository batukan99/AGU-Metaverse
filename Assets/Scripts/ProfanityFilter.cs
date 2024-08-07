using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using TMPro;

public class ProfanityFilter : MonoBehaviour
{
public TMP_Text textDisplay;
public TMP_InputField inputText; 
public ChatBubble chatBubble;
public GlobalChatManager globalChatManager;
public PlayerManager localPlayer;
public TextAsset TextAssetBlockList;

[SerializeField] string[] strBlockList;

    // Start is called before the first frame update
    void Start()
    {
        strBlockList = TextAssetBlockList.text.Split(new string[] {",", "\n"}, StringSplitOptions.RemoveEmptyEntries);
    }

    // Update is called once per frame
    public void CheckInput()
    {
       string profanityChecked = ProfanityCheck(inputText.text);
       
       //inputText.text = ""; 
    }
    
    string ProfanityCheck(string strToCheck)
    {
        for(int i = 0; i < strBlockList.Length; i++)
        {
            string profanity = strBlockList[i];
            System.Text.RegularExpressions.Regex word = new Regex("(?i)(\\b" + profanity + "\\b)");
            string temp = word.Replace(strToCheck, "****");
            strToCheck = temp;
        }
        chatBubble.SendText(strToCheck);
        globalChatManager.SendMessageToChat(PlayerPrefs.GetString("PlayerNickName"), strToCheck, Message.MessageType.PlayerMessage);

        return strToCheck;
    }
}
