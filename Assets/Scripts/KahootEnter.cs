using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class KahootEnter : NetworkBehaviour, IInteractable
{
    [SerializeField] private string interactionLabel;
    public bool entered = false;
    private PlayerManager col;
    public Canvas mainCanvas;
    public GameObject resScreen;
    public GameManager gameManager;
    
    RpcInfo info = default;
    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.tag == "Player") {
            foreach(PlayerManager p in NetworkRunner.FindObjectsOfType<PlayerManager>()) 
            {
              /*  if(p.localPlayer == null){
                    print("not local");
                    continue;
                }
                entered = true;
                col = c.gameObject.GetComponent<PlayerManager>();
                mainCanvas.worldCamera = col.cam;*/
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_InformPlayers(RpcInfo info = default) 
    {
        
        gameManager.enabled = true;
        gameManager.mainCanvas.gameObject.SetActive(true);
        gameManager.resScreen.SetActive(true);
        gameManager.RPC_StartQuiz();
        //col.OnQuizEntered(mainCanvas.gameObject);
        if(PlayerManager.localPlayer != null)
        {
            PlayerManager.localPlayer.GetComponent<PlayerInteract>().keyPressed = true;
            PlayerManager.localPlayer.GetComponent<PlayerInteract>().interactionUI.SetActive(false);
            PlayerManager.localPlayer.OnQuizEntered(mainCanvas, gameManager);
        }
            
    }

    public void GetPlayer(GameObject player)
    {
        //col = player.gameObject.GetComponent<PlayerManager>();
    }
    public void Interact()
    {
        
        RPC_InformPlayers();
        
        foreach(PlayerManager p in NetworkRunner.FindObjectsOfType<PlayerManager>()) 
        {
            if(p.Object.HasInputAuthority)
               { p.OnQuizEntered(mainCanvas, gameManager);
                   // p.RPC_InformPlayers();
               }
        }
        //WgameManager.players.Add(col);
        //gameManager.enabled = true;
        //gameManager.RPC_StartQuiz();
        
    }

    public string GetUIText() {

        return interactionLabel;

    }

    public Transform GetTransform() {
        return transform;
    }



}
