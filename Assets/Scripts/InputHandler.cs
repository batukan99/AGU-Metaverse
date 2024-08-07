using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    public Vector2 moveInputVector = Vector2.zero;
    private GameObject cam;
    public NetworkInputData networkInputData;
    private void Awake()
    {
        cam = GetComponentInChildren<Camera>().gameObject;
    }

    void Update() {
    //Move input
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");
    }


    public NetworkInputData GetNetworkInput()
    {
        networkInputData = new NetworkInputData();

        //Aim data
        networkInputData.aimForwardVector = cam.transform.forward;

        networkInputData.camEulerY = cam.transform.eulerAngles.y;

        //Move data
        networkInputData.movementInput = moveInputVector;

        if (Input.GetKey(KeyCode.Space)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_JUMP, true);
        }
        if (Input.GetKey(KeyCode.W)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_FORWARD, true);
        }

        if (Input.GetKey(KeyCode.S)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_BACKWARD, true);
        }

        if (Input.GetKey(KeyCode.A)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_LEFT, true);
        }

        if (Input.GetKey(KeyCode.D)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_RIGHT, true);
        }
        if (Input.GetKey(KeyCode.E)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_ACTION1, true);
        }

        if (Input.GetKey(KeyCode.Q)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_ACTION2, true);
        }

        if (Input.GetKey(KeyCode.F)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_ACTION3, true);
        }

        if (Input.GetKey(KeyCode.G)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_GESTURE_LIST, true);
        }

        if (Input.GetKey(KeyCode.R)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_RELOAD, true);
        }

        if (Input.GetMouseButton(0)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_FIRE, true);
        }

        if (Input.GetMouseButton(1)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_FIRE_ALT, true);
        }
        
        if (Input.GetKey(KeyCode.LeftShift)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_RUN, true);
        }
        
        if (Input.GetKey(KeyCode.T)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_CHAT, true);
        }
        
        if (Input.GetKey(KeyCode.V)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_VOICE_CHAT, true);
        }
        
        if (Input.GetKey(KeyCode.Return)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_CHAT_SEND, true);
        }
        
        if (Input.GetKey(KeyCode.LeftControl)) {
        networkInputData.Buttons.Set(NetworkInputData.BUTTON_CROUCH, true);
        }
        


        //Reset variables now that we have read their states
        //isJumpButtonPressed = false;

        return networkInputData;
    }
}
