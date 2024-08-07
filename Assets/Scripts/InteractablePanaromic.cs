using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Welcome to 360 image interaction! In order to achieve a 360 image viewing experience, you need to
// use an inverted sphere (under models) and apply a picture that 2:1 with "equirectangular projection" 
// please refer to textures/test_image.jpg
public class InteractablePanaromic : MonoBehaviour, IInteractable
{
    private string interactionLabel;
    [SerializeField] string openLabel; // for setting the label inside the view mode
    [SerializeField] string closeLabel; // for setting the label before view mode
    GameObject mainCamera; // to switch between camera modes (points to cameraBrain)
    bool inInteraction = false; // to set interactionLabel correctly
    [SerializeField] GameObject camBrain; // cambrain of view mode
    private GameObject col;
    void Start() {
        interactionLabel = openLabel; // by default, we are out of interaction
    }
    public void Interact()
    {
        if (!inInteraction) // if outside of interaction, state that we are inside view mode and swap camera
        {
            inInteraction = true;
            interactionLabel = closeLabel;
            mainCamera = Camera.main.transform.parent.gameObject;
            mainCamera.SetActive(false);
            camBrain.SetActive(true);
        }
        else
        { // if inside of interaction, state that we are no longer inside view mode and swap the camera
            interactionLabel = openLabel;
            mainCamera.SetActive(true);
            camBrain.SetActive(false);
            inInteraction = false;
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