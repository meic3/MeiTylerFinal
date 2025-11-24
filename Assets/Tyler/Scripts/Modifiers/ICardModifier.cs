using UnityEngine;

public interface ICardModifier
{
    void OnActivate(Alpaca alpaca);
    void OnDeactivate(Alpaca alpaca);
    void OnUpdate(Alpaca alpaca);
}
