using Gameplay.Cards;
using UnityEngine;
using UI;

namespace Gameplay.Rewards
{
    public class RewardUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RewardController rewardController;
        [SerializeField] private GameObject cardPrefab;

        [Header("Card Display")]
        [SerializeField] private Transform cardContainer;

        public void ShowRewards()
        {
            foreach (CardData cardData in rewardController.CurrentRewards)
            {
                GameObject cardObject = Instantiate(cardPrefab, cardContainer);

                UiCardLink cardLink = cardObject.GetComponent<UiCardLink>();

                if (cardLink == null)
                {
                    Debug.LogError("RewardUI: UiCardLink is missing on card prefab.");
                    Destroy(cardObject);
                    continue;
                }

                Card card = new Card(cardData);
                cardLink.Initialize(card);
            }
        }
    }
}