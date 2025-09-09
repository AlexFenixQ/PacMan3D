using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentGizmo : MonoBehaviour
{
    NavMeshAgent agent;

    void OnValidate()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnDrawGizmos()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        bool onMesh = (agent != null) && agent.isOnNavMesh;
        Gizmos.color = onMesh ? Color.green : Color.red;

        // рисуем шарик чуть выше поверхности
        Gizmos.DrawSphere(transform.position + Vector3.up * 0.1f, 0.25f);
    }
}