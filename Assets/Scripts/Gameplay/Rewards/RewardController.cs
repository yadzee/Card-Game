using System.Collections.Generic;
using Gameplay.Cards;
using UnityEngine;

namespace Gameplay.Rewards
{
    public class RewardController : MonoBehaviour
    {
        [SerializeField] private Deck.Deck deck;
        [SerializeField] private List<CardData> rewardPool = new List<CardData>();

        private readonly List<CardData> _currentRewards = new List<CardData>();

        public IReadOnlyList<CardData> CurrentRewards => _currentRewards;

        public IReadOnlyList<CardData> GenerateRewards(int rewardCount = 3)
        {
            _currentRewards.Clear();

            if (rewardCount <= 0)
            {
                Debug.LogWarning("Reward count must be greater than zero.");
                return _currentRewards;
            }

            // Создаём временный пул без пустых ссылок и повторов.
            List<CardData> availableCards = new List<CardData>();

            foreach (CardData cardData in rewardPool)
            {
                if (cardData != null && !availableCards.Contains(cardData))
                {
                    availableCards.Add(cardData);
                }
            }

            if (availableCards.Count == 0)
            {
                Debug.LogWarning("Reward pool is empty.");
                return _currentRewards;
            }

            // Выбираем случайные карты без повторений.
            int rewardsToGenerate = Mathf.Min(rewardCount, availableCards.Count);

            for (int i = 0; i < rewardsToGenerate; i++)
            {
                int randomIndex = Random.Range(0, availableCards.Count);

                CardData selectedCard = availableCards[randomIndex];

                _currentRewards.Add(selectedCard);
                availableCards.RemoveAt(randomIndex);
            }

            Debug.Log($"Generated {_currentRewards.Count} rewards.");

            return _currentRewards;
        }

        public bool ClaimReward(CardData cardData)
        {
            if (deck == null)
            {
                Debug.LogError("RewardController: Deck reference is missing.");
                return false;
            }

            if (cardData == null || !_currentRewards.Contains(cardData))
            {
                Debug.LogWarning("Selected card is not in the current rewards.");
                return false;
            }

            deck.AddRewardCard(cardData);

            Debug.Log($"Reward claimed: {cardData.Name}");

            _currentRewards.Clear();

            return true;
        }

        public void SkipRewards()
        {
            _currentRewards.Clear();

            Debug.Log("Rewards skipped.");
        }
    }
}
