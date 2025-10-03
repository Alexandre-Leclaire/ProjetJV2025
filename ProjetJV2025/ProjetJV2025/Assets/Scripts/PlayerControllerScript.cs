using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    private CharacterController controller;
    private Interactable currentInteractable;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        // Instant movement - no smoothing
        if (Input.GetKey(KeyCode.W)) movement.z = moveSpeed;
        if (Input.GetKey(KeyCode.S)) movement.z = -moveSpeed;
        if (Input.GetKey(KeyCode.A)) movement.x = -moveSpeed;
        if (Input.GetKey(KeyCode.D)) movement.x = moveSpeed;

        // Apply gravity
        movement.y = -2f;

        controller.Move(movement * moveSpeed * Time.deltaTime);
        
        // Press F to interact
        if (Input.GetKeyDown(KeyCode.F) && currentInteractable)
        {
            currentInteractable.Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            currentInteractable.OnPlayerEnter(); // Generic call!
        }
    }

    void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null && currentInteractable == interactable)
        {
            currentInteractable.OnPlayerExit(); // Generic call!
            currentInteractable = null;
        }
    }
}

