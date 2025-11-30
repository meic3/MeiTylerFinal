using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class SpeedModifier : MonoBehaviour, ICardModifier
{
    [SerializeField] float multiplier = 2f;

    public void OnActivate(Alpaca alpaca)
    {
        alpaca.stats.speed.MultiplyValue(multiplier);
    }
    public void OnDeactivate(Alpaca alpaca)
    {
        alpaca.stats.speed.MultiplyValue(1/multiplier);
    }
    public void OnUpdate(Alpaca alpaca) { }
}
