using Gameplay.Cards;
using Gameplay.Deck;
using UnityEngine;
using Tools.UI.Card;

namespace UI
{
    public class PlayerHandUI : MonoBehaviour
    {
        [SerializeField] private Deck deck;
        [SerializeField] private UiPlayerHand playerHand;
        [SerializeField] private GameObject cardPrefab;

        public void AddCardToHand(Card card)
        {
            GameObject cardObject = Instantiate(cardPrefab, playerHand.transform);

            UiCardLink cardLink = cardObject.GetComponent<UiCardLink>();
            cardLink.Initialize(card);
            Debug.Log($"UI card linked to: {cardLink.Card.Name}");

            IUiCard uiCard = cardObject.GetComponent<IUiCard>();
            playerHand.AddCard(uiCard);
        }
    }
}