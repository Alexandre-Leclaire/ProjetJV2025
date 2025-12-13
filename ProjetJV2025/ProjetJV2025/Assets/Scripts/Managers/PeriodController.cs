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
            ApplyDistribution(ResourceType.Food);
            yield return new WaitForSeconds(periodDuration);

            ApplyDistribution(ResourceType.Cloth);
            yield return new WaitForSeconds(periodDuration);

            ApplyDistribution(ResourceType.Metal);
            yield return new WaitForSeconds(periodDuration);
        }
    }

    void ApplyDistribution(ResourceType dominant)
    {
        foreach (var s in spawners)
        {
            float r = Random.value;

            string id =
                r < 0.6f ? dominant.ToString() :
                r < 0.8f ? "Cloth" :
                           "Metal";

            s.Spawn(id);
        }
    }

}
