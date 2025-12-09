using UnityEngine;

public class AimToMouse : MonoBehaviour
{
    [Header("Réglages")]
    public Camera cam;                     // Caméra isométrique
    public LayerMask groundMask;           // Le layer du sol (à définir dans l’inspecteur)
    public float rotationSpeed = 10f;      // Vitesse de rotation du joueur

    void LateUpdate()
    {
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
        {
            Vector3 targetPoint = hit.point;
            targetPoint.y = transform.position.y;

            Vector3 direction = (targetPoint - transform.position).normalized;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    targetRotation,
                    Time.deltaTime * rotationSpeed
                );
            }
        }
    }
}