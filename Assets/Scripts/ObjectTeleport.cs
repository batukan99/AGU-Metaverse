using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class ObjectTeleport : NetworkBehaviour
{

public Transform teleport;
public bool entered = false;
private GameObject col;
RpcInfo info = default;
    void OnTriggerEnter(Collider c)
    {
        entered = true;
        col = c.gameObject;
        StartCoroutine("Teleport");
    }
        IEnumerator Teleport(){
        if(entered == true){
        col.GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        col.GetComponent<NetworkCharacterControllerPrototype>().TeleportToPosition(teleport.position);
        yield return new WaitForSeconds(0.5f);
        col.GetComponent<CharacterController>().enabled = true;
        entered = false;
        }
    }

}
