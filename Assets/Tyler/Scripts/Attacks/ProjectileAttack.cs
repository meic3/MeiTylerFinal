using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour, IAttack
{

    public GameObject projectilePrefab;

    // shoot projectiles at bug(s)
    public void Attack(Alpaca alpaca)
    {
        var bugs = new List<GameObject>(alpaca.rangeHelper.bugsInRange);
        int attacks = alpaca.stats.attackCount.IntValue < bugs.Count ? alpaca.stats.attackCount.IntValue : bugs.Count;
        for (int i = 0; i < attacks; i++)
        {
            GameObject target = bugs[i];

            //Debug.Log(dir);
            Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
            projectile.Init(target, alpaca);
        }

    }
}
