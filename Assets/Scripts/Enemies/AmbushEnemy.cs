using UnityEngine;
using UnityEngine.AI;

public class AmbushEnemy : MonoBehaviour
{
    public Transform ambushPoint;
    public float chaseDistance = 5f;

    private NavMeshAgent agent;
    private Transform player;
    private bool chasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        if (!EnsureOnNavMesh()) Debug.LogWarning($"{name}: agent not on NavMesh after sampling.");

        if (agent.isOnNavMesh && ambushPoint != null) agent.SetDestination(ambushPoint.position);
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

        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (!chasing && dist <= chaseDistance) chasing = true;

        if (chasing) agent.SetDestination(player.position);
        else if (ambushPoint != null) agent.SetDestination(ambushPoint.position);
    }

    bool EnsureOnNavMesh()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (agent == null) return false;

        if (!agent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
                return true;
            }
            return false;
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.PlayerDied();
            AudioManager.Instance.PlayDeath();
        }
    }
}