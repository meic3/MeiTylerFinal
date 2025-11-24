using UnityEngine;

public class DamageModifier : MonoBehaviour, ICardModifier
{
    [SerializeField] float multiplier = 2f;

    public void OnActivate(Alpaca alpaca)
    {
        alpaca.mDamage *= multiplier;
    }
    public void OnDeactivate(Alpaca alpaca)
    {
        alpaca.mDamage /= multiplier;
    }
    public void OnUpdate(Alpaca alpaca) { }
}
