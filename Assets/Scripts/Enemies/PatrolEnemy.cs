using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemy : MonoBehaviour
{
    public Transform[] waypoints;
    public float waitTime = 1f;

    private NavMeshAgent agent;
    private int currentIndex = 0;
    private float waitTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        EnsureOnNavMesh();

        if (agent.isOnNavMesh && waypoints != null && waypoints.Length > 0)
            agent.SetDestination(waypoints[0].position);
    }

    void Update()
    {
        if (agent == null) return;
        if (!agent.isOnNavMesh) { agent.isStopped = true; return; }

        if (GameManager.Instance == null || GameManager.Instance.State != GameState.Playing)
        {
            agent.isStopped = true;
            return;
        }
        agent.isStopped = false;

        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                currentIndex = (currentIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[currentIndex].position);
                waitTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.PlayerDied();
            AudioManager.Instance.PlayDeath();
        }
    }

    void EnsureOnNavMesh()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (agent == null) return;

        if (!agent.isOnNavMesh)
        {
            NavMeshHit hit;
            // ищем ближайшую точку NavMesh в радиусе 5
            if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
                // agent автоматически должен быть "на" NavMesh после перемещения
            }
        }
    }
}