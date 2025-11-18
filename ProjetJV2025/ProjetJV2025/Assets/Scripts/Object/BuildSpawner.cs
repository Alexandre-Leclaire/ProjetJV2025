using System;
using TMPro;
using UnityEngine;

public class BuildSpawner : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_Text buildNameText;
    [SerializeField] private GameObject[] buildPrefabs;

    private int curIndex = 0;
    private float timer = 2f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas.SetActive(false);
        buildNameText.text = buildPrefabs[curIndex].name;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.SetActive(true);
            canvas.transform.LookAt(Camera.main.transform);
        }

        if (timer <= 0f)
        {
            timer = 2f;
            curIndex = (curIndex + 1) % buildPrefabs.Length;
        }
        else
        {
            timer -= Time.deltaTime;
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
        Instantiate(buildPrefabs[curIndex], position, Quaternion.identity);
        Destroy(gameObject);
    }
}