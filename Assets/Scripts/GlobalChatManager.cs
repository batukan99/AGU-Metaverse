using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;
public class GlobalChatManager : Fusion.NetworkBehaviour
{
    public int maxMessages = 8;
    public GameObject chatPanel, textObject;
    public Color playerMessage, info;

    [SerializeField]
    List<Message> messageList = new List<Message>();

/*
    void Update()
    {
        if (chatbox.text != "")
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat(username + ": " + chatbox.text, Message.MessageType.playerMessage);
                chatbox.text = "";
            }
        }
        else
        {
            if(!chatbox.isFocused && Input.GetKeyDown(KeyCode.Return))
            chatbox.ActivateInputField();
        }
        if(chatbox.isFocused)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            { 
            SendMessageToChat("You pressed the space bar", Message.MessageType.info);
            }
        }
    }*/

    public void SendMessageToChat(string username, string text, Message.MessageType messageType){
        RPC_SendMessageToChat(username, text, messageType);
     }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendMessageToChat(string username, string text, Message.MessageType messageType, RpcInfo info = default){
        if(messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].tmp.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();
        
    
        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.tmp = newText.GetComponent<TextMeshProUGUI>();
        messageList.Add(newMessage);
        newMessage.tmp.text = username + ": " + text;

        newMessage.tmp.color = MessageTypeColor(messageType);

        
    }
    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = info;
        switch (messageType)
        {
            case Message.MessageType.PlayerMessage:
            color = playerMessage;
            break;
        }
        return color;
    }
}

