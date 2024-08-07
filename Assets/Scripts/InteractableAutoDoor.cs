using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InteractableAutoDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private string interactionLabel;
    [SerializeField] string message;

    private GameObject col;

    //[Networked] [HideInInspector]
    public bool isOpen = false;
    
    public void Interact()
    {
        Debug.Log(message);
        isOpen = true;
    }
    
    public void FixedUpdate() {
        if (isOpen) {
            animator.SetTrigger("Open");
            isOpen = false;
        }
    }
    

    public string GetUIText()
    {

        return interactionLabel;

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
