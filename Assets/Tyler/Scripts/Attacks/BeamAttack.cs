using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BeamAttack : MonoBehaviour, IAttack
{
    // beam pierce by default
    // chain has no effect

    float distance = 30f;
    public LineRenderer beamPrefab;


    public void Attack(Alpaca alpaca)
    {
        var bugs = new List<GameObject>(alpaca.rangeHelper.bugsInRange);
        int attacks = alpaca.stats.attackCount.IntValue < bugs.Count ? alpaca.stats.attackCount.IntValue : bugs.Count;
        for (int i = 0; i < attacks; i++)
        {
            GameObject target = bugs[i];
            Vector3 dir = (target.transform.position - transform.position).normalized;

            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(.2f, .2f), 0, dir, distance);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("bug"))
                {
                    HitBug(hit.collider.GetComponent<Bug>(), alpaca);
                }
            }

            LineRenderer lr = Instantiate(beamPrefab, transform.position, Quaternion.identity);

            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + dir * distance);

            Destroy(lr.gameObject, 0.2f);
        }
    }

    private void HitBug(Bug bug, Alpaca alpaca)
    {
        // bug take damage
        bug.bugLife.TakeDamage(alpaca.stats.damage.value, alpaca.stats.cull.value);

        // slow
        if (alpaca.stats.slow.value > 0)
        { bug.AddSlowInstance(alpaca.stats.slow.value, 2f); } // default 2 second slow for now
    }
}
