using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.UI;
using Photon.Voice.Unity;

public class ChatBubble : Fusion.NetworkBehaviour
{
    public TextMeshPro usernameText;
    public TextMeshPro textMeshPro;
    public TMP_InputField field;
    public GameObject bubble;
    public Transform cam;
    public RawImage chatImage;
    public Texture Icon1;
    public Texture Icon2;
    public RawImage voiceImage;
    public Texture voiceIcon1;
    public Texture voiceIcon2;
    public bool isChatting;
    public bool isMuted;
    private GlobalChatManager globalChatManager;
    public Speaker speaker;
    public CameraMovement cameraMovement;
    private void Awake()
    {
        //textMeshPro = bubble.transform.Find("Text").GetComponent<TextMeshPro>();
        //field = transform.Find("Text").GetComponent<TMP_InputField>();
        //cam = transform.Find("Camera").transform;
    }
    
    public void Start(){
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        textMeshPro.gameObject.SetActive(true);
        bubble.gameObject.SetActive(true);
        textMeshPro.SetText(message);
        textMeshPro.ForceMeshUpdate();
        StartCoroutine(RemoveAfterSeconds(textMeshPro.gameObject, 5));
        StartCoroutine(RemoveAfterSeconds(bubble.gameObject, 5));
    }

    void Update()
    {
        
        //textMeshPro.transform.LookAt(cam);
    }

    public void SetUsernameText(string name) {
        usernameText.SetText(name);
        usernameText.ForceMeshUpdate();
        //RPC_SetUsernameText(name);
    }
    public void SendText(string text) {
        //Setup(field.text);
        if (Object.HasInputAuthority)
        {
            RPC_SendMessage(text);
        }
    }

    public void EnableOrDisableChat(){
        if(!globalChatManager){
        globalChatManager = NetworkRunner.FindObjectOfType<GlobalChatManager>();
        }
        if(isChatting == true){
            field.gameObject.SetActive(false);
            cameraMovement.camLock = false;
            isChatting = false;
            chatImage.texture = Icon1;
            globalChatManager.chatPanel.SetActive(false);
        }
        else if(isChatting == false){
            field.gameObject.SetActive(true);
            cameraMovement.camLock = true;
            isChatting = true;
            chatImage.texture = Icon2;
            globalChatManager.chatPanel.SetActive(true);
        }
    }
    public void EnableOrDisableVoice(){

        if(isMuted == true){
            speaker.enabled = true;
            isMuted = false;
            voiceImage.texture = voiceIcon1;
        }
        else if(isMuted == false){
            speaker.enabled = false;
            isMuted = true;
            voiceImage.texture = voiceIcon2;
        }
    }

    IEnumerator RemoveAfterSeconds(GameObject obj, int seconds)
    {
            yield return new WaitForSeconds(seconds);
            obj.SetActive(false);
    }
}
