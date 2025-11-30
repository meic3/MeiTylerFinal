using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Alpaca alpaca;

    float projectileSpeed = 20f;
    float lifeTime = 5f;
    Rigidbody2D rb;


    #region Should be called when instantiated/shot

    // set constant velocity towards direction, rotate towards direction
    public void SetDir(Vector3 dir)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * projectileSpeed;

        transform.up = dir;
    }

    public void SetAlpaca(Alpaca alpaca)
    {
        this.alpaca = alpaca;

    }

    #endregion





    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("bug"))
        {
            Bug bug = col.GetComponent<Bug>();
            if (bug != null) HitBug(bug);
        }
    }

    private void HitBug(Bug bug)
    {
        bug.bugLife.TakeDamage(alpaca.stats.damage.value);
        if (alpaca.stats.slow.value > 0)
        { bug.AddSlowInstance(alpaca.stats.slow.value, 2f); } // default 2 second slow for now

        Destroy(gameObject); // to do: might hit other bugs before projectile is destroyed
    }
}
