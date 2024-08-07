using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerInteract : MonoBehaviour
{

    [SerializeField] private float interactRange = 5f;
    [SerializeField] public GameObject interactionUI;
    public bool keyPressed = false;

    // Update is called once per frame
    void Update()
    {
        Collider[] colliding = Physics.OverlapSphere(transform.position, interactRange);
        List<IInteractable> interactablesList = new List<IInteractable>();
        foreach (Collider collider in colliding)
        {
            if (collider.gameObject != gameObject && collider.TryGetComponent(out IInteractable interactable)) interactablesList.Add(interactable);
        }
        IInteractable closest = null;
        foreach (IInteractable interactable in interactablesList)
        {
            //snake code go brr
            //closest = (closest == null) ? interactable : (Vector3.Distance(transform.position, interactable.transform.position) < Vector3.Distance(transform.position, interactable.transform.position) ? interactable : closest);
            //if (closest == null || Vector3.Distance(transform.position, interactable.transform.position) < Vector3.Distance(transform.position, interactable.transform.position))

            //this way sqroot is not needed, more optimised!!!
            if (closest == null || (transform.position - interactable.GetTransform().position).sqrMagnitude < (transform.position - interactable.GetTransform().position).sqrMagnitude)
            {
                closest = interactable;
            }
        }
        if (closest != null)
        {
            if(!keyPressed)
            interactionUI.SetActive(true);
            interactionUI.GetComponentInChildren<TextMeshProUGUI>().SetText(closest.GetUIText());
            if (Input.GetKeyDown(KeyCode.E))
            {
                closest.GetPlayer(gameObject);
                 closest.Interact();

                keyPressed = true;
                interactionUI.SetActive(false);
            }
        } else {
            interactionUI.SetActive(false);
            keyPressed = false;
        }
    }
}