using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour {

    [HideInInspector] public Interactable CurrentInteractable;

    public LayerMask    InteractLayer;
    public float        InteractDist;
    public Text         InteractText;

    public void Interact ()
    {
        if (CurrentInteractable) CurrentInteractable.OnInteract();
    }

    private void Update()
    {
        RaycastHit hit;
        Ray newRay = new Ray(PlayerManager._Camera.transform.position, PlayerManager._Camera.transform.forward);
        if (Physics.Raycast(newRay, out hit, InteractDist, InteractLayer))
        {
            if (!CurrentInteractable)
            {
                CurrentInteractable = hit.collider.GetComponent<Interactable>();
                CurrentInteractable.OnEnter();
                InteractText.enabled = true;
                InteractText.text = CurrentInteractable.InteractMessage;

            }
        }
        else
        {
            if (CurrentInteractable)
            {
                CurrentInteractable.OnExit();
                InteractText.enabled = false;
                CurrentInteractable = null;
            }
        }
    }
}
