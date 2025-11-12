using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemie : MonoBehaviour, IAiEntity
{
    [SerializeField] private float patrolRadius;
    [SerializeField] private float vision;
    [SerializeField] [Range (0f, 180f)]private float visionAngle;
    [SerializeField] private float shootRange;
    [SerializeField] private float shootTime;
    [SerializeField] private GameObject player;
    [SerializeField] private BulletMovement bulletPrefab;
    [SerializeField] private GameObject laserObject;
    
    private Vector3 startPosition;
    private NavMeshAgent agent;
    private IState state;

    public NavMeshAgent Agent => agent;

    public IState State { get => state; set => state = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        startPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        Agent.SetDestination(transform.position);
        State = new PatrolState(this);

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        bulletPrefab.Init(10, shootRange);

        laserObject.transform.localScale = new Vector3(0.1f, 0.1f, shootRange);
        laserObject.transform.localPosition = new Vector3(0, 0, (shootRange / 2) + 0.5f);
    }

    // Update is called once per frame
    private void Update()
    {
        State.Execute();
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

    private bool HavePlayerInVision()
    {
        Vector3 playerDirection = player.transform.position - transform.position;

        // Debug.Log($"Distance: {Vector3.Distance(transform.position, player.transform.position)}, Angle: {Vector3.Angle(playerDirection, transform.forward)}");
        return Vector3.Distance(transform.position, player.transform.position) <= vision &&
               Vector3.Angle(playerDirection, transform.forward) <= visionAngle &&
               Physics.Raycast(transform.position, playerDirection, out var hit, vision) &&
               hit.transform.gameObject == player;
    }

    private class PatrolState : IState
    {
        public IAiEntity Entity => enemie;
        private Enemie enemie;
        
        public PatrolState(Enemie enemie)
        {
            // Debug.Log("Patrol");
            enemie.Agent.stoppingDistance = 0;
            this.enemie = enemie;
        }
        void IState.Execute()
        {
            if (enemie.HavePlayerInVision())
            {
                enemie.Agent.SetDestination(enemie.player.transform.position);
                enemie.state = new ChaseState(enemie);
            }
            else if (enemie.Agent.remainingDistance <= 0.1f)
            {
                enemie.Agent.SetDestination(enemie.RandomNavmeshLocation(enemie.patrolRadius));
            }
        }
    }
    private class ChaseState : IState
    {
        public IAiEntity Entity => enemie;
        private Enemie enemie;

        public ChaseState(Enemie enemie)
        {
            // Debug.Log("Chase");
            this.enemie = enemie;

            enemie.Agent.stoppingDistance = enemie.shootRange;
        }

        void IState.Execute()
        {
            if (enemie.HavePlayerInVision())
            {
                enemie.Agent.SetDestination(enemie.player.transform.position);
                if (Vector3.Distance(enemie.player.transform.position, enemie.transform.position) <= enemie.shootRange)
                {
                    enemie.State = new ShootState(enemie, enemie.shootTime);
                }
            }
            else if (enemie.Agent.remainingDistance <= enemie.Agent.stoppingDistance)
            {
                enemie.State = new PatrolState(enemie);
            }
        }
    }
    private class ShootState : IState
    {
        public IAiEntity Entity => entity;
        private Enemie entity;
        private float shootTimer;
        public ShootState(Enemie enemie, float shootTimer)
        {
            // Debug.Log("Shoot");
            enemie.transform.LookAt(enemie.player.transform);
            enemie.Agent.isStopped = true;
            this.shootTimer = shootTimer;
            this.entity = enemie;
            entity.laserObject.SetActive(true);
        }

        void IState.Execute()
        {
            if (shootTimer <= 0)
            {
                Instantiate(entity.bulletPrefab, entity.transform.position, entity.transform.rotation);
                entity.Agent.isStopped = false;
                entity.laserObject.SetActive(false);
                entity.State = new ChaseState(entity);
            }
            else
            {
                shootTimer -= Time.deltaTime;
            }
        }
    }
}
