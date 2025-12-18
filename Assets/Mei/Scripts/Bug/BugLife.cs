using UnityEngine;
using System.Collections;

public class BugLife : MonoBehaviour
{
    public float maxHP = 10f;
    public float currentHP;
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
        StartCoroutine(FlashRed());
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

        

        BugGenerator generator = FindObjectOfType<BugGenerator>();
        if (generator != null)
        {
            generator.OnBugKilled();
            if (generator.particle != null)
            {
                ParticleSystem ps = Instantiate(generator.particle, transform.position, Quaternion.identity);
                ps.Play();
            }
        }
        Destroy(gameObject);
    }

    public IEnumerator FlashRed()
    {
        SpriteRenderer bugSpr = GetComponent<SpriteRenderer>();
        bugSpr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        bugSpr.color = Color.white;
    }
}
