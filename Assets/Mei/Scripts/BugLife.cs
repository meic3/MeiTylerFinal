using UnityEngine;

public class BugLife : MonoBehaviour
{
    public float maxHP = 10f;
    private float currentHP;
    public int moneyReward = 5;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PlayerMoney.money += moneyReward;

        BulletShooter shooter = FindAnyObjectByType<BulletShooter>();
        if (shooter != null)
            shooter.ClearTarget(transform);

        Destroy(gameObject);
    }
}
