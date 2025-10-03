using TMPro;
using UnityEngine;

public class DoorInteractable : Interactable
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
            interactText.text = "Press F to get out";
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
        Debug.Log("Get out of the door!");
        // Chest logic
    }
}
