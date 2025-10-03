using System;
using TMPro;
using UnityEngine;

public class ChestInteractable : Interactable
{
    public TextMeshProUGUI interactText;

    private void Start()
    {
        interactText.gameObject.SetActive(false);
    }

    public override void OnPlayerEnter()
    {
        if (interactText != null)
        {
            interactText.text = "Press F to open chest";
            interactText.gameObject.SetActive(true);
        }
    }
    
    public override void OnPlayerExit()
    {
        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }
    }
    
    public override void Interact()
    {
        Debug.Log("Opening chest!");
        // Chest logic
    }
}
