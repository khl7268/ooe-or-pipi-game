using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float hp = 50;

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        if (hp <= 0) Destroy(gameObject);
    }
}