using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 30f, -30f); // 45 degree angle
    public float rotationSpeed = 45f; // Degrees to rotate per press
    
    private float currentRotation = 0f;

    void LateUpdate()
    {
        if (player)
        {
            // Rotate camera with Q and E
            if (Input.GetKeyDown(KeyCode.Q))
            {
                currentRotation -= rotationSpeed;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentRotation += rotationSpeed;
            }
            
            // Apply rotation to offset
            Quaternion rotation = Quaternion.Euler(0f, currentRotation, 0f);
            Vector3 rotatedOffset = rotation * offset;
            
            // Follow player position with rotated offset
            transform.position = player.position + rotatedOffset;
            
            // Always look at player
            transform.LookAt(player);
        }
    }
}