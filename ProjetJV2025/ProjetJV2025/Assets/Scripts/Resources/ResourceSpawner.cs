using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject prefab;
    public float respawnTime = 45f;

    GameObject current;
    string currentResourceId;

    public void Spawn(string resourceId)
    {
        currentResourceId = resourceId;

        if (current != null)
            Destroy(current);

        current = Instantiate(prefab, transform.position, Quaternion.identity);

        var pickup = current.GetComponent<ResourcePickup>();
        pickup.Init(this, resourceId);

        var color = current.GetComponent<ResourceCubeColor>();
        if (color != null)
            color.SetColor(resourceId);
    }

    public void OnCollected()
    {
        current = null;
        Invoke(nameof(Respawn), respawnTime);
    }

    void Respawn()
    {
        Spawn(currentResourceId);
    }
}
