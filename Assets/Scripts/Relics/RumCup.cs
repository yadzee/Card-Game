using UnityEngine;

public class RumCup : MonoBehaviour
{
    [SerializeField] private int _triggerThreshold = 6;
    private int _playedCards;
    private RumCupState _currentRumCupState;
    private CardEffect _chosenEffect;
    private const float EffectMultiplier = 2f;

    public bool IsActive => _currentRumCupState != RumCupState.Inactive;

    public void RegisterCardPlayed()
    {
        if (_currentRumCupState != RumCupState.Inactive)
            return;

        _playedCards++;

        if (_playedCards >= _triggerThreshold)
            ActivateRumCupRelic();
    }

    private void ActivateRumCupRelic()
    {
        _playedCards = 0;
        _currentRumCupState = RumCupState.WaitingForCard;

        Debug.Log("Activating RumCupRelic");
    }

    public bool IsEffectSuitable(CardEffect effect)
    {
        if (_currentRumCupState != RumCupState.WaitingForCard)
            return false;

        return effect is DamageEffect || effect is BlockEffect;
    }

    public CardEffect ChooseCard(Card card)
    {
        if (_currentRumCupState != RumCupState.WaitingForCard)
            return null;

        foreach (var effect in card.CardEffects)
        {
            bool isSuitable = IsEffectSuitable(effect);
            if (isSuitable)
            {
                _chosenEffect = effect;
                _currentRumCupState = RumCupState.Inactive;
                return effect;
            }
        }

        return null;
    }

    public float GetEffectMultiplier(CardEffect effect)
    {
        if (effect == _chosenEffect)
            return EffectMultiplier;

        return 1f;
    }

    public void ConsumeBonus()
    {
        _chosenEffect = null;
    }

    public void ResetAfterTurn()
    {
        if (_currentRumCupState == RumCupState.WaitingForCard)
            _currentRumCupState = RumCupState.Inactive;
        Debug.Log($"RumCup state after turn: {_currentRumCupState}");
    }

private enum RumCupState
    {
        Inactive,
        WaitingForCard
    }
}