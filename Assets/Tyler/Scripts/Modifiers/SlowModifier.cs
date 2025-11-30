using UnityEngine;

public class SlowModifier : MonoBehaviour, ICardModifier
{
    [SerializeField] float slow = .3f;

    public void OnActivate(Alpaca alpaca)
    {
       alpaca.stats.slow.AddValue(slow);
    }
    public void OnDeactivate(Alpaca alpaca)
    {
       alpaca.stats.slow.AddValue(-slow);
    }
    public void OnUpdate(Alpaca alpaca) { }
}
