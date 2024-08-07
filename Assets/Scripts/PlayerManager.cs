using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerManager : NetworkBehaviour
{
    [Networked (OnChanged = nameof(OnNicknameChanged))]
    public NetworkString<_16> username {get; set;}
    [Networked(OnChanged = nameof(OnHeadIndexChanged))] public int headIndex {get; set;}
    [Networked(OnChanged = nameof(OnBodyIndexChanged))]  public int bodyIndex {get; set;}
    [Networked(OnChanged = nameof(OnLegIndexChanged))]  public int legIndex {get; set;}
    [Networked(OnChanged = nameof(OnFootIndexChanged))]  public int footIndex {get; set;}
    public GameObject localUI;
    public static PlayerManager localPlayer;
    public ProfanityFilter profanityFilter;
    public Camera cam;
    public ChatBubble chatBubble;
    public List<PlayerManager> otherPlayers = new List<PlayerManager>();
 
    public GameManager gameManager;
    public BodyPartHandler bodyPartHandler;
    public uint uid;
    
    public override void Spawned() {
        base.Spawned();

        if(Object.HasInputAuthority){
         localPlayer = this;
         profanityFilter.localPlayer = this;

         //username = PlayerPrefs.GetString("PlayerNickName");
         //chatBubble.SetUsernameText(username.ToString());

         RPC_SetNickname(PlayerPrefs.GetString("PlayerNickName"));
         
         //load body parts
         RPC_SetHeadIndex(PlayerPrefs.GetInt(CharacterCustomizationManager.headData));
         RPC_SetBodyIndex(PlayerPrefs.GetInt(CharacterCustomizationManager.bodyData));
         RPC_SetLegIndex(PlayerPrefs.GetInt(CharacterCustomizationManager.legData));
         RPC_SetFootIndex(PlayerPrefs.GetInt(CharacterCustomizationManager.footData));

        //profanityFilter.globalChatManager = GameObject.FindWithTag("GlobalChat").GetComponent<GlobalChatManager>();

         profanityFilter.globalChatManager = NetworkRunner.FindObjectOfType<GlobalChatManager>();
         gameManager = NetworkRunner.FindObjectOfType<GameManager>();

            foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("Player")) {
                if(fooObj == localPlayer.gameObject)
                    return;
                PlayerManager p = fooObj.GetComponent<PlayerManager>();
                p.chatBubble.transform.parent.GetComponent<UILookAt>().cam = cam;
                //p.chatBubble.SetUsernameText(p.username.ToString());
                otherPlayers.Add(p);
            }
            foreach(InteractableNPC p in NetworkRunner.FindObjectsOfType<InteractableNPC>()) {
                print("asdf");
               // InteractableNPC p = fooObj.GetComponent<InteractableNPC>();
               p.chatBubble.transform.parent.GetComponent<UILookAt>().cam = cam;
                p.bubbleCanvas.worldCamera = cam;
            }
        }
        else {
            localUI.SetActive(false);
            foreach(PlayerManager p in NetworkRunner.FindObjectsOfType<PlayerManager>()) {
                // if this object has not input authority we should find the local player
                // which means, find the local player and assign local player's camera to this player's chat bubble lookAt 
                if(p == localPlayer) 
                    {
                        chatBubble.transform.parent.GetComponent<UILookAt>().cam = p.cam;
                        localPlayer = this;
                    }
            }
        }
        

    }

    public override void FixedUpdateNetwork()
    {
       /* if(voice != null && voice.isJoined && localPlayer != null) {
            foreach(PlayerManager p in otherPlayers){
            voice.UpdateSpatialAudioPosition(p.uid, p.gameObject.transform.position);
            p.voice.UpdateSpatialAudioPosition(uid, gameObject.transform.position);
            }
        }*/
        
    }
    public void OnBikeExited(BikeMovement1 bm1)
    {
        RPC_BikeExited(bm1);
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_BikeExited(BikeMovement1 bm1, RpcInfo info = default)
    { 
        bm1.LeaveBike();
        bm1.rider = null;
        bm1.getInput = false;
    }

    public void OnBikeEntered(BikeMovement1 bm1)
    {
        RPC_Bike(bm1);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_Bike(BikeMovement1 bm1, RpcInfo info = default)
    { 
        bm1.rider = transform;
        bm1.getInput = !bm1.getInput;
    }

    public void OnQuizEntered(Canvas quiz, GameManager gameManager)
    {
        
        cam.transform.parent.GetComponent<CameraMovement>().camLock = true;
        cam.transform.parent.gameObject.GetComponent<CameraMovement>().FocusToQuiz(quiz.transform);
        quiz.worldCamera = cam;
        gameObject.GetComponent<NetworkCharacterControllerPrototype>().moveLock = true;
        
    }
    
    public void OnQuizExited()
    {
        cam.transform.parent.GetComponent<CameraMovement>().camLock = false;
        gameObject.GetComponent<NetworkCharacterControllerPrototype>().moveLock = false;
        cam.transform.localEulerAngles = new Vector3(0,270,0);
    }

    static void OnNicknameChanged(Changed<PlayerManager> changed){
        changed.Behaviour.OnNicknameChanged();
    }
    private void OnNicknameChanged(){
        chatBubble.SetUsernameText(username.ToString());
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetNickname(string name, RpcInfo info = default)
    {
        this.username = name;
    }

    //for head
    static void OnHeadIndexChanged(Changed<PlayerManager> changed){
        changed.Behaviour.OnHeadIndexChanged();
    }
    private void OnHeadIndexChanged(){
        bodyPartHandler.ChangeHead(headIndex);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetHeadIndex(int index, RpcInfo info = default)
    {
        this.headIndex = index;
    }

    //for body
    static void OnBodyIndexChanged(Changed<PlayerManager> changed){
        changed.Behaviour.OnBodyIndexChanged();
    }
    private void OnBodyIndexChanged(){
        bodyPartHandler.ChangeBody(bodyIndex);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetBodyIndex(int index, RpcInfo info = default)
    {
        this.bodyIndex = index;
    }

    //for leg
    static void OnLegIndexChanged(Changed<PlayerManager> changed){
        changed.Behaviour.OnLegIndexChanged();
    }
    private void OnLegIndexChanged(){
        bodyPartHandler.ChangeLeg(legIndex);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetLegIndex(int index, RpcInfo info = default)
    {
        this.legIndex = index;
    }

    //for foot
    static void OnFootIndexChanged(Changed<PlayerManager> changed){
        changed.Behaviour.OnFootIndexChanged();
    }
    private void OnFootIndexChanged(){
        bodyPartHandler.ChangeFoot(footIndex);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetFootIndex(int index, RpcInfo info = default)
    {
        this.footIndex = index;
    }
}
