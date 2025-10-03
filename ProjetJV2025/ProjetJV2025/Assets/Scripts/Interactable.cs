using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    public virtual void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
    
    public virtual void OnPlayerEnter()
    {
        // Override this in child classes to show UI
    }
    
    public virtual void OnPlayerExit()
    {
        // Override this in child classes to hide UI
    }
}