using UnityEngine;
using UnityEngine.AI;

public class PippiController : MonoBehaviour
{
    private NavMeshAgent agent;

    public float attackRange = 5f;
    public float attackDamage = 10f;
    public float attackDelay = 1f;

    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        FindEnemyAndAttack();
    }

    public void MoveTo(Vector3 target)
    {
        agent.SetDestination(target);
    }

    void FindEnemyAndAttack()
{
    Debug.Log("탐색 중");

    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    foreach (var enemy in enemies)
    {
        float dist = Vector3.Distance(agent.nextPosition, enemy.transform.position);

        

        if (dist <= attackRange)
{
    agent.isStopped = true;

    // 방향 맞추기 🔥
    Vector3 dir = (enemy.transform.position - transform.position).normalized;
    transform.rotation = Quaternion.LookRotation(dir);

    EnemyController ec = enemy.GetComponent<EnemyController>();

    if (ec != null)
    {
        ec.TakeDamage(attackDamage);
    }

    return;
}
    }

    agent.isStopped = false;
}
}