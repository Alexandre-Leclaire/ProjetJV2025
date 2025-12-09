using UnityEngine;
using System.Collections;

public class ResourceSpawner : MonoBehaviour
{
    public Item.ItemType currentType;
    public GameObject foodPrefab;
    public GameObject clothPrefab;
    public GameObject metalPrefab;

    GameObject spawned;
    float respawnTime = 45f;

    public void Spawn(Item.ItemType type)
    {
        currentType = type;
        if (spawned != null) Destroy(spawned);

        GameObject prefab = type == Item.ItemType.Food ? foodPrefab :
                            type == Item.ItemType.Cloth ? clothPrefab :
                            metalPrefab;

        spawned = Instantiate(prefab, transform.position, Quaternion.identity);

        var pickup = spawned.GetComponent<ResourcePickup>();
        pickup.spawner = this;
        pickup.type = type;
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        Spawn(currentType);
    }
}
