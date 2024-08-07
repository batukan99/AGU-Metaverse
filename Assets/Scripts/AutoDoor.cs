using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    [SerializeField] private float doorRange = 3f;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        bool doClose = true;
        Collider[] colliding = Physics.OverlapSphere(transform.position, doorRange);
        foreach (Collider collider in colliding)
        {
        if (collider.gameObject != gameObject && collider.tag == "Player") {
                gameObject.GetComponent<Animator>().SetBool("Open", true);
                doClose = false;
                // Do stuff
                return;
            }  
        }
        if (doClose) gameObject.GetComponent<Animator>().SetBool("Open", false);
    }
}
