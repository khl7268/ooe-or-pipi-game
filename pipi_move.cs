using UnityEngine;
using UnityEngine.AI;

public class pipi_move : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 target)
    {
        agent.SetDestination(target);
    }
}