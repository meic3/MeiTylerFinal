using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour, IAttack
{

    public GameObject projectilePrefab;
    public Type[] compatibleModifiers = new Type[]
        {
            typeof(AOEModifier), // only work with projectile with aoe splash at the end. maybe make bullet bigger? or have seperate AOEProjectileAttack class
            typeof(ChainModifier),
            typeof(CullModifier),
            typeof(DamageModifier),
            typeof(HomingProjectileModifier), // kind scuffed
            typeof(PierceModifier),
            typeof(RangeModifier),
            typeof(SlowModifier),
            typeof(SpeedModifier),
            typeof(AttackCountModifier)
        };

    // shoot projectiles at bug(s)
    public void Attack(Alpaca alpaca)
    {
        if (projectilePrefab.name == "Bullet")
        {SFXManager.Instance.PlaySound(SFXManager.SoundType.BulletGun);}
        if (projectilePrefab.name == "Spit")
        {SFXManager.Instance.PlaySound(SFXManager.SoundType.BulletSpit);}
        if (projectilePrefab.name == "Fireball")
        {SFXManager.Instance.PlaySound(SFXManager.SoundType.BulletFire);}
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

    public Type[] CompatibleModifiers()
    {
        return compatibleModifiers;
    }
}
