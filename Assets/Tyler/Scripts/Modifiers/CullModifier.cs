using UnityEngine;

public class CullModifier : MonoBehaviour, ICardModifier
{
    [SerializeField] float value = 1f;

    public void OnActivate(Alpaca alpaca)
    {
        alpaca.stats.cull.AddValue(value);
    }
    public void OnDeactivate(Alpaca alpaca)
    {
        alpaca.stats.cull.AddValue(-value);
    }
    public void OnUpdate(Alpaca alpaca) { }
}
