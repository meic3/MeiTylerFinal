using UnityEngine;

public class AOEProjectile : Projectile
{
    float AOERadius = 1f;
    public float AOEDamageMultiplier = 0.5f;
    public GameObject AOEEffectPrefab;

    public override void Init(GameObject target, Alpaca alpaca)
    {
        base.Init(target, alpaca);
        AOERadius = alpaca.stats.AOE.value;
    }

    protected override void HitBug(Bug bug)
    {
        base.HitBug(bug);
        HitBugsAOE();
    }

    private void HitBugsAOE()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, AOERadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("bug"))
            {
                Bug bug = hit.GetComponent<Bug>();
                if (bug != null && !bug.bugLife.Died)
                {
                    bug.bugLife.TakeDamage(alpaca.stats.damage.value * AOEDamageMultiplier, alpaca.stats.cull.value);
                    
                    // slow
                    if (alpaca.stats.slow.value > 0)
                    { bug.AddSlowInstance(alpaca.stats.slow.value, 2f); } // default 2 second slow for now
                }
            }
        }

        if (AOEEffectPrefab != null)
        {
            AOEEffectHelper helper = Instantiate(AOEEffectPrefab, transform.position, Quaternion.identity).GetComponent<AOEEffectHelper>();
            helper.radius = AOERadius;
        }
    }
}
