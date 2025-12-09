using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Laser : MonoBehaviour
{
    public Transform pointOfFire;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        var laser = Physics.Raycast(transform.position, transform.forward, out hit, 20f);
        if (hit.collider == null)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 20f);
            return;
        }
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, hit.distance);
        Debug.Log(hit.distance);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (hit.distance - 20f));
    }
}