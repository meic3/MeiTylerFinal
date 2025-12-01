using UnityEngine;

public class HomingProjectileModifier : MonoBehaviour, ICardModifier
{

    public void OnActivate(Alpaca alpaca)
    {
        alpaca.stats.homingProjectile = true;
    }
    public void OnDeactivate(Alpaca alpaca)
    {
        alpaca.stats.homingProjectile = false;
    }
    public void OnUpdate(Alpaca alpaca) { }
}