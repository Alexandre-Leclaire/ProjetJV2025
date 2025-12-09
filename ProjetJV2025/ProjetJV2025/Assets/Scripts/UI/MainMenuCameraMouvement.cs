using UnityEngine;
using System.Collections;

public class MainMenuCameraMouvement : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;

    public void GoTo(Transform destination)
    {
        StopAllCoroutines();
        StartCoroutine(GoToPosition(destination));
    }

    private IEnumerator GoToPosition(Transform destination)
    {
        float distance = Vector3.Distance(transform.position, destination.position);
        float angleDistance = Vector3.Distance(transform.forward, destination.forward);

        while (destination.position != transform.position || destination.rotation != transform.rotation)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, Time.deltaTime * distance);

            Vector3 targetDirection = destination.position - transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, destination.forward, Time.deltaTime * angleDistance, 0.0f));

            yield return new WaitForEndOfFrame();
        }
    }
}
