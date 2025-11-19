using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildSpawner : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_Text buildNameText;
    [SerializeField] private TMP_Text bottomText;
    [SerializeField] private GameObject[] buildPrefabs;
    [SerializeField] private float spawnTimer = 20f;
    [SerializeField] private float cooldownTimer = 90f;

    private int curIndex = 0;
    private float timer = 2f;
    private bool inCooldown = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas.SetActive(false);
        buildNameText.text = buildPrefabs[curIndex].name;
        bottomText.text = "E";
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"Build Spawner: {other.name}");
        if (other.CompareTag("Player"))
        {
            canvas.SetActive(true);
            canvas.transform.LookAt(Camera.main.transform);

            if (!inCooldown)
            {
                if (timer <= 0f)
                {
                    timer = 2f;
                    curIndex = (curIndex + 1) % buildPrefabs.Length;
                    buildNameText.text = buildPrefabs[curIndex].name;
                }
                else
                {
                    timer -= Time.deltaTime;
                }

                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    SpawnBuild();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.SetActive(false);
        }
    }

    public void SpawnBuild()
    {
        var position = new Vector3(transform.position.x, 0, transform.position.z);
        var build = Instantiate(buildPrefabs[curIndex], transform);
        inCooldown = true;

        StartCoroutine(CooldownCoroutine(build));
    }

    private IEnumerator CooldownCoroutine(GameObject build)
    {
        float time = spawnTimer;

        while (time > 0)
        {
            time -= Time.deltaTime;
            bottomText.text = $"{(int)time}s before despawn.";
            yield return new WaitForEndOfFrame();
        }

        Destroy(build);

        time = cooldownTimer;
        while (time > 0)
        {
            time -= Time.deltaTime;
            bottomText.text = $"Usable in {(int)time}s.";
            yield return new WaitForEndOfFrame();
        }

        inCooldown = false;
        bottomText.text = "E";
    }
}