using UnityEngine;
using UnityEngine.AI;

public class PippiController : MonoBehaviour
{
    NavMeshAgent agent;
    public float hp = 50;
    public float attackRange = 3f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;
    
    public bool isPlayerTeam = true;
    public bool targetTeamIsPlayer = false;
    
    private float despawnDelay = 2f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = gameObject.AddComponent<NavMeshAgent>();
        }
    }

    void Update()
    {
        if (GameManager.instance.currentState != GameManager.GameState.Battle)
            return;
        
        // 적 찾기
        GameObject[] enemies = FindTargets();

        if (enemies.Length == 0)
        {
            return;
        }
        
        // 가장 가까운 적 찾기
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;
        
        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;
            
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestEnemy = enemy;
            }
        }
        
        if (closestEnemy != null)
        {
            // 공격 범위 내면 공격
            if (closestDistance < attackRange)
            {
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    Attack(closestEnemy);
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                // 범위 밖이면 이동
                agent.SetDestination(closestEnemy.transform.position);
            }
        }
    }
    
    GameObject[] FindTargets()
    {
        string targetTag = isPlayerTeam ? "Enemy" : "Player";
        return GameObject.FindGameObjectsWithTag(targetTag);
    }
    
    void Attack(GameObject target)
    {
        BibiController bibiController = target.GetComponent<BibiController>();
        if (bibiController != null)
        {
            bibiController.TakeDamage(attackDamage);
        }
        else
        {
            PippiController pippiController = target.GetComponent<PippiController>();
            if (pippiController != null)
                pippiController.TakeDamage(attackDamage);
        }
    }

    public void MoveTo(Vector3 pos)
    {
        agent.SetDestination(pos);
    }

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        if (hp <= 0) 
        {
            Die();
        }
    }
    
    void Die()
    {
        GameManager.instance.currentUnitCount--;
        Destroy(gameObject, despawnDelay);
    }
}