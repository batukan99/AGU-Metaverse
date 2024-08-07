using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;
public class InteractableNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionLabel;
    [SerializeField] private string message;
    [SerializeField] private Dialogue dialogueScreen;
    public ChatBubble chatBubble;
    public Canvas bubbleCanvas;
    public DialogueOption[] options; // The current set of dialogue options
    private GameObject col;
    RpcInfo info = default;
    public void GetPlayer(GameObject player)
    {
        col = player;
    }
    IEnumerator Teleport(Transform teleport)
    {
        col.GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        col.GetComponent<NetworkCharacterControllerPrototype>().TeleportToPosition(teleport.position);
        yield return new WaitForSeconds(0.5f);
        col.GetComponent<CharacterController>().enabled = true;
        dialogueScreen.CloseDialogueScreen();
    }

    public void Interact()
    {
        dialogueScreen.gameObject.SetActive(true);
        dialogueScreen.StartDialogue(options, message, this);
        Debug.Log(message); 
        chatBubble.SendText(message);
        
    }

    public string GetUIText() {

        return interactionLabel;

    }

    public Transform GetTransform() {
        return transform;
    }
}
