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
        // Give player money
        PlayerMoney.money += moneyReward;

        // Tell shooter target is gone
        BulletShooter shooter = FindAnyObjectByType<BulletShooter>();
        if (shooter != null)
            shooter.ClearTarget(transform);

        Destroy(gameObject);
    }
}
