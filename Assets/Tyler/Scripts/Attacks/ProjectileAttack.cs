using UnityEngine;

public class ProjectileAttack : MonoBehaviour, IAttack
{

    public GameObject prefab;

    // shoot projectiles at bug(s)
    public void Attack(Alpaca alpaca)
    {
        int attacks = alpaca.stats.attackCount.IntValue < alpaca.rangeHelper.bugsInRange.Count ? alpaca.stats.attackCount.IntValue : alpaca.rangeHelper.bugsInRange.Count;
        for (int i=0; i<attacks; i++)
        {
            GameObject target = alpaca.rangeHelper.bugsInRange[i];
            Vector3 dir = target.transform.position - transform.position;
            dir = dir.normalized;

            Debug.Log(dir);
            Projectile projectile = Instantiate(prefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
            projectile.SetDir(dir);
            projectile.SetAlpaca(alpaca);
        }

    }
}
