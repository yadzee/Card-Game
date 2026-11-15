
using Gameplay.Cards;
using UnityEngine;
using Gameplay.Combat;
using UI;

namespace Gameplay.Rewards
{
    public class RewardUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RewardController rewardController;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private CombatController combatController;

        [Header("Reward Screen")]
        [SerializeField] private GameObject rewardPanel;
        [SerializeField] private GameObject rewardBackground;
        [SerializeField] private GameObject rewardForegroundCanvas;

        [Header("Card Display")]
        [SerializeField] private Transform[] cardSlots;

        private bool _isRewardResolved;

        private void Awake()
        {
            if (combatController == null)
            {
                Debug.LogError("RewardUI: CombatController reference is missing.");
                return;
            }

            combatController.OnCombatEnded += HandleCombatEnded;

            SetRewardScreenVisible(false);
        }

        private void HandleCombatEnded(CombatState state)
        {
            if (state != CombatState.Victory)
                return;

            ShowRewards();
        }

        private void OnDestroy()
        {
            if (combatController != null)
                combatController.OnCombatEnded -= HandleCombatEnded;
        }

        public void ShowRewards()
        {
            if (rewardController == null ||
                cardPrefab == null ||
                cardSlots == null ||
                rewardPanel == null ||
                rewardBackground == null ||
                rewardForegroundCanvas == null)
            {
                Debug.LogError("RewardUI: Missing references in Inspector.");
                return;
            }

            _isRewardResolved = false;
            ClearRewards();

            int rewardCount = rewardController.CurrentRewards.Count;

            if (rewardCount == 0)
            {
                Debug.LogWarning("RewardUI: No rewards to display.");
                return;
            }

            SetRewardScreenVisible(true);

            if (rewardCount > cardSlots.Length)
            {
                Debug.LogWarning("RewardUI: Not enough card slots for all rewards.");
            }

            int cardsToDisplay = Mathf.Min(rewardCount, cardSlots.Length);

            for (int i = 0; i < cardsToDisplay; i++)
            {
                if (cardSlots[i] == null)
                {
                    Debug.LogError($"RewardUI: Card slot {i} is missing.");
                    continue;
                }

                CardData cardData = rewardController.CurrentRewards[i];

                GameObject cardObject = Instantiate(cardPrefab, cardSlots[i]);

                UiCardLink cardLink = cardObject.GetComponent<UiCardLink>();

                if (cardLink == null)
                {
                    Debug.LogError("RewardUI: UiCardLink is missing on card prefab.");
                    Destroy(cardObject);
                    continue;
                }

                Card card = new Card(cardData);
                cardLink.Initialize(card);

                RewardCardSelector selector =
                    cardObject.GetComponent<RewardCardSelector>();

                if (selector == null)
                {
                    Debug.LogError("RewardUI: RewardCardSelector is missing on card prefab.");
                    Destroy(cardObject);
                    continue;
                }

                selector.Initialize(cardData);
                selector.OnSelected += HandleRewardSelected;
            }

            Debug.Log($"RewardUI: Displayed {cardsToDisplay} rewards.");
        }

        private void SetRewardScreenVisible(bool isVisible)
        {
            rewardPanel.SetActive(isVisible);
            rewardBackground.SetActive(isVisible);
            rewardForegroundCanvas.SetActive(isVisible);
        }

        private void ClearRewards()
        {
            if (cardSlots == null)
                return;

            foreach (Transform slot in cardSlots)
            {
                if (slot == null)
                    continue;

                foreach (Transform child in slot)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        private void HandleRewardSelected(CardData cardData)
        {
            if (_isRewardResolved)
                return;

            if (rewardController.ClaimReward(cardData))
            {
                _isRewardResolved = true;

                ClearRewards();
                SetRewardScreenVisible(false);

                Debug.Log($"RewardUI: Selected reward - {cardData.Name}");
            }
        }
        
        public void HandleSkipButtonClicked()
        {
            if (_isRewardResolved)
                return;

            if (rewardController == null)
            {
                Debug.LogError("RewardUI: RewardController reference is missing.");
                return;
            }

            rewardController.SkipRewards();

            _isRewardResolved = true;

            ClearRewards();
            SetRewardScreenVisible(false);

            Debug.Log("RewardUI: Rewards skipped.");
        }
    }
}
