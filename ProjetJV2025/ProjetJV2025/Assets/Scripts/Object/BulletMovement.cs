using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private float speed;
    private float maxDistance;
    private int damage;
    
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    public void Init(float speed, float maxDistance, int damage)
    {
        this.speed = speed;
        this.maxDistance = maxDistance;
        this.damage = damage;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector3.Distance(transform.position, _startPosition) > maxDistance)
        {
            Destroy(gameObject);
        }
        
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log("Hit");
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
