using UnityEngine;
using UnityEngine.AI;

public class Enemie : MonoBehaviour
{
    [SerializeField] private float patrolRadius;
    [SerializeField] private float vision;
    [SerializeField] private float shootRange;
    [SerializeField] private GameObject player;
    
    private Vector3 startPosition;
    private NavMeshAgent agent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        startPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(transform.position);

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (agent.remainingDistance <= 0.1f)
        {
            agent.SetDestination(RandomNavmeshLocation(patrolRadius));
        }

        if (player != null)
        {
            Vector3 playerDirection = player.transform.position - transform.position;
            Debug.DrawRay(transform.position, playerDirection, Color.red);

            if (Physics.Raycast(transform.position, playerDirection, out var hit, vision) &&    
                hit.transform.gameObject == player)
            {
                agent.SetDestination(hit.point);

                if (Vector3.Distance(transform.position, hit.point) <= shootRange)
                {
                    agent.isStopped = true;
                    Shoot();
                }
                else
                {
                    agent.isStopped = false;
                }
            }
        }
    }
    
    private Vector3 RandomNavmeshLocation(float radius) {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += startPosition;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
            finalPosition = hit.position;            
        }
        return finalPosition;
    }

    private void Shoot()
    {
        Debug.Log("Piou!");
    }
}
