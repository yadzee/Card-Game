
using System.Collections.Generic;
using Gameplay.Cards;
using Gameplay.Deck;
using UnityEngine;

namespace UI
{
    public class DeckViewer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Deck deck;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private Transform cardsParent;
        [SerializeField] private GameObject viewerPanel;

        [Header("Layout")]
        [SerializeField] private int columns = 5;
        [SerializeField] private Vector2 spacing = new Vector2(7f, 9f);

        public void ShowDrawPile()
        {
            ShowCards(deck.DrawPile);
        }

        public void ShowDiscardPile()
        {
            ShowCards(deck.DiscardPile);
        }

        private void ShowCards(IReadOnlyList<Card> cards)
        {
            Clear();

            if (cards == null || cardPrefab == null || cardsParent == null)
            {
                Debug.LogError("DeckViewer: Missing references.");
                return;
            }

            viewerPanel.SetActive(true);

            int safeColumns = Mathf.Max(1, columns);

            for (int i = 0; i < cards.Count; i++)
            {
                int row = i / safeColumns;
                int column = i % safeColumns;

                int cardsInRow = Mathf.Min(
                    safeColumns,
                    cards.Count - row * safeColumns
                );

                float x = (column - (cardsInRow - 1) * 0.5f) * spacing.x;
                float y = -row * spacing.y;

                GameObject cardObject = Instantiate(
                    cardPrefab,
                    cardsParent,
                    false
                );

                cardObject.transform.localPosition = new Vector3(x, y, 0f);

                UiCardLink cardLink = cardObject.GetComponent<UiCardLink>();

                if (cardLink == null)
                {
                    Debug.LogError("DeckViewer: UiCardLink is missing.");
                    Destroy(cardObject);
                    continue;
                }

                cardLink.Initialize(cards[i]);
            }
        }

        public void Close()
        {
            Clear();
            viewerPanel.SetActive(false);
        }

        private void Clear()
        {
            if (cardsParent == null)
                return;

            foreach (Transform child in cardsParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
