using UnityEngine;

public class RangeModifier : MonoBehaviour, ICardModifier
{
    [SerializeField] float value = 1f;

    public void OnActivate(Alpaca alpaca)
    {
        alpaca.aRange += value;
    }
    public void OnDeactivate(Alpaca alpaca)
    {
        alpaca.aRange -= value;
    }
    public void OnUpdate(Alpaca alpaca) { }
}
