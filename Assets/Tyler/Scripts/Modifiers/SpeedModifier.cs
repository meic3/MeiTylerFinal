using UnityEngine;

public class SpeedModifier : MonoBehaviour, ICardModifier
{
    [SerializeField] float multiplier = 2f;

    public void OnActivate(Alpaca alpaca)
    {
        alpaca.mSpeed *= multiplier;
    }
    public void OnDeactivate(Alpaca alpaca)
    {
        alpaca.mSpeed /= multiplier;
    }
    public void OnUpdate(Alpaca alpaca) { }
}
