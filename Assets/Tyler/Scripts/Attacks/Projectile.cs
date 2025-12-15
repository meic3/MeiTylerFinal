using UnityEngine;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public Alpaca alpaca;

    GameObject target;
    bool isHoming = false;

    int chainedCount = 0; // have already chained how many times
    int piercedCount = 0;
    List<GameObject> prevTargets = new List<GameObject>(); // cannot chain to bugs already hit by this projectile


    public float projectileSpeed = 20f;
    float lifeTime = 5f;
    Rigidbody2D rb;
    public bool rotateTowardsVelocity = true;

    bool canHitBug = true;

    // set constant velocity towards direction, rotate towards direction

    // Should be called when instantiated/shot
    public virtual void Init(GameObject target, Alpaca alpaca)
    {
        this.target = target;
        this.alpaca = alpaca;
        isHoming = alpaca.stats.homingProjectile;

        rb = GetComponent<Rigidbody2D>();
        AimAtTarget();
    }

    // set velocity and rotation towards the targeted bug
    private void AimAtTarget()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir = dir.normalized;

        rb.linearVelocity = dir * projectileSpeed;

        if (rotateTowardsVelocity) transform.up = dir;
    }



    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0) Destroy(gameObject);

        if (isHoming && target != null)
        {
            AimAtTarget();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("bug"))
        {
            canHitBug = false;
            Bug bug = col.GetComponent<Bug>();
            if (bug != null && !bug.bugLife.Died) HitBug(bug);
        }
    }

    protected virtual void HitBug(Bug bug)
    {
        prevTargets.Add(bug.gameObject);

        // bug take damage
        bug.bugLife.TakeDamage(alpaca.stats.damage.value, alpaca.stats.cull.value);

        // slow
        if (alpaca.stats.slow.value > 0)
        { bug.AddSlowInstance(alpaca.stats.slow.value, 2f); } // default 2 second slow for now

        // chain
        if (alpaca.stats.chain.value > 0 && chainedCount < alpaca.stats.chain.value)
        {
            chainedCount++;
            isHoming = true;
            target = FindClosestBug();
            canHitBug = true;
        }
        // pierce
        else if (alpaca.stats.pierce.value > 0 && piercedCount < alpaca.stats.pierce.value)
        {
            piercedCount++;
            isHoming = false;
            canHitBug = true;
        }
        // projectiles uses all chains before piercing

        else Destroy(gameObject);
    }

    // find closest bug that hasnt been hit by this projectile
    // could be optimized by having an bugmanager keeping a list of all bugs
    private GameObject FindClosestBug()
    {
        GameObject[] bugs = GameObject.FindGameObjectsWithTag("bug");
        GameObject closestBug = null;
        float minDistanceSqr = Mathf.Infinity;

        foreach (GameObject bug in bugs)
        {
            if (prevTargets.Contains(bug)) continue;
            float distanceSqr = (transform.position - bug.transform.position).sqrMagnitude;
            if (distanceSqr < minDistanceSqr)
            {
                minDistanceSqr = distanceSqr;
                closestBug = bug;
            }
        }
        return closestBug;
    }
}
