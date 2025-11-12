using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxDistance;
    
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    public void Init(float speed, float maxDistance)
    {
        this.speed = speed;
        this.maxDistance = maxDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, _startPosition) > maxDistance)
        {
            Destroy(gameObject);
        }
        
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            Destroy(gameObject);
        }
    }
}
