using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PeriodController : MonoBehaviour
{
    public List<ResourceSpawner> spawners;
    public float periodDuration = 120f;

    void Start()
    {
        StartCoroutine(Cycle());
    }

    IEnumerator Cycle()
    {
        while (true)
        {
            ApplyDistribution(Item.ItemType.Food);
            yield return new WaitForSeconds(periodDuration);

            ApplyDistribution(Item.ItemType.Cloth);
            yield return new WaitForSeconds(periodDuration);

            ApplyDistribution(Item.ItemType.Metal);
            yield return new WaitForSeconds(periodDuration);
        }
    }

    void ApplyDistribution(Item.ItemType dominant)
    {
        foreach (var s in spawners)
        {
            float r = Random.value;

            if (r < 0.6f)
                s.Spawn(dominant);
            else if (r < 0.8f)
                s.Spawn(Item.ItemType.Cloth);
            else
                s.Spawn(Item.ItemType.Metal);
        }
    }
}
