using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PeriodController : MonoBehaviour
{
    //[Header("Spawners")]
    private ResourceSpawner[] spawners;

    [Header("Period settings")]
    public float periodDuration = 120f;

    public ResourceType CurrentPeriod { get; private set; }

    void Start()
    {
        ResourceType[] periods = (ResourceType[])System.Enum.GetValues(typeof(ResourceType));
        CurrentPeriod = periods[Random.Range(0, periods.Length)];

        spawners = FindObjectsByType<ResourceSpawner>(FindObjectsSortMode.None);

        StartCoroutine(Cycle());
    }

    IEnumerator Cycle()
    {
        while (true)
        {
            ApplyCurrentPeriod();
            yield return new WaitForSeconds(periodDuration);

            GoToNextPeriod();
        }
    }

    void GoToNextPeriod()
    {
        switch (CurrentPeriod)
        {
            case ResourceType.Food:
                CurrentPeriod = ResourceType.Cloth;
                break;

            case ResourceType.Cloth:
                CurrentPeriod = ResourceType.Metal;
                break;

            case ResourceType.Metal:
                CurrentPeriod = ResourceType.Food;
                break;
        }
    }

    void ApplyCurrentPeriod()
    {
        int total = spawners.Length;

        int dominantCount = Mathf.RoundToInt(total * 0.50f);
        int secondaryCount = Mathf.RoundToInt(total * 0.35f);
        int rareCount = total - dominantCount - secondaryCount;

        ResourceType secondary = GetSecondary(CurrentPeriod);
        ResourceType rare = GetRare(CurrentPeriod);

        List<ResourceType> distribution = new List<ResourceType>();

        for (int i = 0; i < dominantCount; i++)
            distribution.Add(CurrentPeriod);

        for (int i = 0; i < secondaryCount; i++)
            distribution.Add(secondary);

        for (int i = 0; i < rareCount; i++)
            distribution.Add(rare);

        Shuffle(distribution);

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].Spawn(distribution[i].ToString());
        }

        Debug.Log($"[PeriodController] New period: {CurrentPeriod}");
    }

    void Shuffle(List<ResourceType> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            ResourceType temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    ResourceType GetSecondary(ResourceType dominant)
    {
        switch (dominant)
        {
            case ResourceType.Food: return ResourceType.Cloth;
            case ResourceType.Cloth: return ResourceType.Metal;
            case ResourceType.Metal: return ResourceType.Food;
            default: return ResourceType.Food;
        }
    }

    ResourceType GetRare(ResourceType dominant)
    {
        switch (dominant)
        {
            case ResourceType.Food: return ResourceType.Metal;
            case ResourceType.Cloth: return ResourceType.Food;
            case ResourceType.Metal: return ResourceType.Cloth;
            default: return ResourceType.Metal;
        }
    }
}
