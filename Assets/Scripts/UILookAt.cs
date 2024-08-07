using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAt : MonoBehaviour
{
    // Start is called before the first frame update
    
    // Update is called once per frame
    public Camera cam;
    
    void LateUpdate()
    {
        if(cam != null)
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }
}
