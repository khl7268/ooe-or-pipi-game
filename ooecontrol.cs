using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float hp = 50f;
    public float attackRange = 3f;
    public float attackDamage = 10f;
    public float attackDelay = 1f;

    private float lastAttackTime;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        FindTargetAndAttack();
    }

    void FindTargetAndAttack()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Unit");

        foreach (var target in targets)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);

            if (dist <= attackRange)
            {
                agent.isStopped = true;

                // 방향 맞추기
                Vector3 dir = (target.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(dir);

                if (Time.time - lastAttackTime > attackDelay)
                {
                    PippiController pc = target.GetComponent<PippiController>();

                    if (pc != null)
                    {
                        pc.TakeDamage(attackDamage);
                        lastAttackTime = Time.time;
                    }
                }

                return;
            }
        }

        agent.isStopped = false;
    }

    public void TakeDamage(float dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}