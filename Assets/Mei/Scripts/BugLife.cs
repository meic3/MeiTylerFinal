using UnityEngine;

public class BugLife : MonoBehaviour
{
    public float maxHP = 10f;
    private float currentHP;
    public int moneyReward = 5;
    public bool Died = false;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    { TakeDamage(damage, 0); }
    public void TakeDamage(float damage, float cull)
    {
        currentHP -= damage;

        if (currentHP <= cull)
        {
            Die();
        }
    }

    void Die()
    {
        Died = true;

        PlayerMoney.money += moneyReward;

        BulletShooter shooter = FindAnyObjectByType<BulletShooter>();
        if (shooter != null)
            shooter.ClearTarget(transform);

        Destroy(gameObject);
    }
}
