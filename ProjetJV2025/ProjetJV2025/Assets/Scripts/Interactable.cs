using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    public GameObject interactIndicator; // The canvas/UI indicator
    
    void Start()
    {
        // Hide indicator on start
        if (interactIndicator != null)
        {
            interactIndicator.SetActive(false);
        }
    }
    
    public virtual void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
    
    public virtual void OnPlayerEnter()
    {
        if (interactIndicator != null)
        {
            interactIndicator.SetActive(true);
        }
    }
    
    public virtual void OnPlayerExit()
    {
        if (interactIndicator != null)
        {
            interactIndicator.SetActive(false);
        }
    }
}