using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

// THIS IS THE EXAMPLE CODE FOR INTERACTABLE OBJECT TO REFERENCE, PLEASE DO NOT CHANGE WITHOUT NOTICE
public class InteractableBicycle : NetworkBehaviour, IInteractable
{
    [SerializeField] private string interactionLabel;
    [SerializeField] string message;
    [SerializeField] BikeMovement1 bm1;
    private GameObject col;
    public void Interact()
    {
        Debug.Log(message);
        
        if(bm1.rider != null)
            col.GetComponent<PlayerManager>().OnBikeExited(bm1);
        else
            col.GetComponent<PlayerManager>().OnBikeEntered(bm1);

       // bm1.LeaveBike();

        col.GetComponent<CharacterController>().enabled = !col.GetComponent<CharacterController>().enabled;
        ControllerPrototype controllerPrototype = col.GetComponent<ControllerPrototype>();
        controllerPrototype.state = (controllerPrototype.state != ControllerPrototype.State.Ride) ? ControllerPrototype.State.Ride : ControllerPrototype.State.Walk;

        //col.GetComponent<CapsuleCollider>().enabled = !col.GetComponent<CapsuleCollider>().enabled;
        // Do stuff such as animations
        // gameObject.GetComponent<Animator>().SetTrigger("Interact");
    }
  /*  [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Bike( RpcInfo info = default)
    { 
         bm1.LeaveBike();
        bm1.getInput = !bm1.getInput;
        bm1.rider = col.transform;
        col.GetComponent<CharacterController>().enabled = !col.GetComponent<CharacterController>().enabled;
        ControllerPrototype controllerPrototype = col.GetComponent<ControllerPrototype>();
        controllerPrototype.state = (controllerPrototype.state != ControllerPrototype.State.Ride) ? ControllerPrototype.State.Ride : ControllerPrototype.State.Walk;
    }*/

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