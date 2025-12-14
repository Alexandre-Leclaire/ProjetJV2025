using System;
using UnityEngine;

public class ResourcePickup : MonoBehaviour
{
    public string resourceId;
    public int amount = 10;

    ResourceSpawner spawner;

    public void Init(ResourceSpawner spawner, string resourceId)
    {
        this.spawner = spawner;
        this.resourceId = resourceId;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Inventory inv = other.GetComponent<Inventory>();
        if (inv == null)
            return;

        inv.Add(resourceId, amount);

        if (spawner != null)
        {
            spawner.OnCollected();
        }

        Destroy(gameObject);
        Debug.Log($"[ResourcePickup] Collected {amount} x {resourceId}");

    }
}
