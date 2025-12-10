using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildSpawner : MonoBehaviour, IActionnable
{
    [SerializeField] private GameObject[] buildPrefabs;
    [SerializeField] private float spawnTimer = 20f;
    [SerializeField] private float cooldownTimer = 90f;

    public List<IActionnable.Element> GetActions()
    {
        List<IActionnable.Element> elements = new List<IActionnable.Element>();

        foreach (var build in buildPrefabs)
        {
            var e = new IActionnable.Element
            {
                name = build.name,
                cancelOnLeave = false,
                resetCooldown = cooldownTimer,
                interactionTime = spawnTimer
            };

            GameObject spawnedBuild = null;
            e.onInteract = _ =>
            {
                spawnedBuild = Instantiate(build, transform.position, Quaternion.identity);
                return true;
            };
            e.onInteractionFinish = _ =>
            {
                Destroy(spawnedBuild);
            };

            elements.Add(e);
        }

        return elements;
    }
}