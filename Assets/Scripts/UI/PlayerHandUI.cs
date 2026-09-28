using System;
using Gameplay.Cards;
using Gameplay.Deck;
using UnityEngine;
using Tools.UI.Card;
using System.Collections;
using System.Collections.Generic;

namespace UI
{
    public class PlayerHandUI : MonoBehaviour
    {
        [SerializeField] private Deck deck;
        [SerializeField] private UiPlayerHand playerHand;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private Transform deckPosition;
        [SerializeField] private Transform gameView;
        [SerializeField] private float cardDrawDelay = 0.2f;
        private IUiPlayerHand _playerHand;
        
        private void Awake()
        {
            _playerHand = playerHand;
            _playerHand.OnCardSelected += HandleCardSelected;
        }

        public void AddCardToHand(Card card)
        {
            var cardObject = Instantiate(cardPrefab, gameView);
            cardObject.transform.position = deckPosition.position;

            UiCardLink cardLink = cardObject.GetComponent<UiCardLink>();
            cardLink.Initialize(card);
            Debug.Log($"UI card linked to: {cardLink.Card.Name}");

            IUiCard uiCard = cardObject.GetComponent<IUiCard>();
            playerHand.AddCard(uiCard);
        }
        
        public void AddCardsToHand(IReadOnlyList<Card> cards)
        {
            StartCoroutine(AddCardsToHandRoutine(cards));
        }

        private IEnumerator AddCardsToHandRoutine(IReadOnlyList<Card> cards)
        {
            foreach (var card in cards)
            {
                AddCardToHand(card);
                yield return new WaitForSeconds(cardDrawDelay);
            }
        }
        
        private void HandleCardSelected(IUiCard uiCard)
        {
            var cardLink = uiCard.MonoBehavior.GetComponent<UiCardLink>();
            Debug.Log($"Selected gameplay card: {cardLink.Card.Name}");
            OnCardSelected?.Invoke(cardLink.Card);
        }
        
        public event Action<Card> OnCardSelected;
    }
}