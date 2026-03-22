using UnityEngine;
using UnityEngine.AI;

public class BibiController : MonoBehaviour
{
    NavMeshAgent agent;
    public float hp = 60f;
    public float attackRange = 3f;
    public float attackDamage = 15f;
    public float attackCooldown = 1.5f;
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
        GameObject[] allUnits = FindTargets();
        
        if (allUnits.Length == 0)
        {
            // 적이 없으면 대기
            return;
        }
        
        // 가장 가까운 적 찾기
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;
        
        foreach (var unit in allUnits)
        {
            if (unit == null) continue;
            
            float dist = Vector3.Distance(transform.position, unit.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestEnemy = unit;
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
        PippiController pippiController = target.GetComponent<PippiController>();
        if (pippiController != null)
        {
            pippiController.TakeDamage(attackDamage);
        }
        else
        {
            BibiController bibiController = target.GetComponent<BibiController>();
            if (bibiController != null)
                bibiController.TakeDamage(attackDamage);
        }
    }
    
    public void TakeDamage(float damage)
    {
        hp -= damage;
        
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
