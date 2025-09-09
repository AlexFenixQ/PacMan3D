using UnityEngine;
using UnityEngine.AI;

public class DirectChaseEnemy : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        // optional placement:
        if (!agent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
                transform.position = hit.position;
        }
    }

    void Update()
    {
        if (agent == null || player == null) return;
        if (GameManager.Instance == null || GameManager.Instance.State != GameState.Playing) return;
        if (!agent.isOnNavMesh) return;

        agent.SetDestination(player.position);
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