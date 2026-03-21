using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 50f;

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        Debug.Log("엉엉이 HP: " + hp);

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}