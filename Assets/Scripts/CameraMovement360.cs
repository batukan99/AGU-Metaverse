using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CameraMovement360 controls the view mode of 360 image viewing.

public class CameraMovement360 : MonoBehaviour
{
    public float sensitivity = 1f; // sensitivity of mouse while using view mode
    public float zoomSpeed = 5; // controls how fast camera will move towards player via scrolling
    public bool invert; // to allow inverting the camera inside view mode
    [SerializeField] InteractablePanaromic interactablePanaromic; // used to allow users exit via pressing ESC
    [SerializeField] Camera camera360; // camera of the brain to change fov

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // when esc is pressed, swap the camera. It works since this code is only enabled inside view mode.
        {
            interactablePanaromic.Interact();
        }
    }

    void LateUpdate()
    {
        float x = Input.GetAxis("Mouse X") * (sensitivity + 0.7f); //0.7 makes the sensitivity reasonable, while keeping visible sensitivity 1 which is more understandable and user friendly
        float y = Input.GetAxis("Mouse Y") * (invert ? -1 : 1) * -(sensitivity + 0.7f);

        //late update araştır
        float rotateZ = transform.eulerAngles.z + y;

        if (rotateZ > 180 && rotateZ < 280) // there is an unresolved problem with eulerAngles, this is a workaround for limiting view angles
                                            // 90 = ground, 0 = forward, 270 = top (this is the problem, no idea why it works like this)
        {
            rotateZ = 280; //-10
        }
        else if (rotateZ > 80 && rotateZ < 180)
        {
            rotateZ = 80;
        }

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + x, rotateZ); // the rotation itself happens here

        if (Input.GetAxis("Mouse ScrollWheel") > 0) //scroll to zoom in/out
        { 
                camera360.fieldOfView = Mathf.Clamp(camera360.fieldOfView - zoomSpeed, 30, 75); // scroll up = zoom in (fov down)
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                camera360.fieldOfView = Mathf.Clamp(camera360.fieldOfView + zoomSpeed, 30, 75); // scroll down = zoom out (fov up)
                
            }
        }
    }
}
