using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InteractableManualDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private string closedInteractionLabel; 
    [SerializeField] private string openedInteractionLabel;
    [SerializeField] string message;

    [SerializeField] private Animator animator;
    private GameObject col;
    //[Networked] [HideInInspector] 
    public bool isOpen = false;
    //[Networked] [HideInInspector] 
    public bool animate = false;
    public void Interact()
    {
        // Do stuff
        Debug.Log(message);
        animate = true;
        isOpen = !isOpen;    
    }

    public void FixedUpdate() {

        if (animate) {
            if (isOpen) {
                animator.SetTrigger("Close");
            }
            else {
                animator.SetTrigger("Open");
            }
            animate = false;
        }
    }

    public string GetUIText()
    {

        return isOpen ? openedInteractionLabel : closedInteractionLabel;

    }

    public Transform GetTransform()
    {
        return transform;
    }
    public void GetPlayer(GameObject player)
    {
        col = player;
    }
}
