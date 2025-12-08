using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public Item.ItemType currentType;
    public GameObject basePrefab;

    GameObject spawned;
    float respawnTime = 45f;

    public void Spawn(Item.ItemType type)
    {
        currentType = type;

        if (spawned != null)
            Destroy(spawned);

        spawned = Instantiate(basePrefab, transform.position, Quaternion.identity);

        var pickup = spawned.GetComponent<ResourcePickup>();
        pickup.spawner = this;
        pickup.type = type;

        var color = spawned.GetComponent<ResourceCubeColor>();
        if (color != null)
            color.SetColor(type);
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        Spawn(currentType);
    }
}
