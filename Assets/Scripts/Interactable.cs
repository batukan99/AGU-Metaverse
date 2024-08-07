using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// THIS IS THE EXAMPLE CODE FOR INTERACTABLE OBJECT TO REFERENCE, PLEASE DO NOT CHANGE WITHOUT NOTICE
public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionLabel;
    [SerializeField] string message;

    
    public GameObject col;
    public void Interact()
    {
        Debug.Log(message);
        // Do stuff such as animations
        // gameObject.GetComponent<Animator>().SetTrigger("Interact");
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