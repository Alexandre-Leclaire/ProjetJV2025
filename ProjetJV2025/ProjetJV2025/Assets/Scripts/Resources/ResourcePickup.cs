using UnityEngine;

public class ResourcePickup : MonoBehaviour
{
    public ResourceSpawner spawner;
    public Item.ItemType type;
    public int amount = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inv = other.GetComponent<Inventory>();

            if (inv != null)
            {
                inv.AddItem(type, amount);
            }

            Destroy(gameObject);
            spawner.StartCoroutine(spawner.Respawn());
        }
    }
}
