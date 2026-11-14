using System;
using System.Collections.Generic;
using Gameplay.Cards;
using Gameplay.Combat;
using Gameplay.Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RewardPanelUI : MonoBehaviour
    {
        [Serializable]
        private class RewardOption
        {
            public Button button;
            public TMP_Text cardName;
            public TMP_Text cardDescription;
            public Image cardArtwork;
        }

        [SerializeField] private CombatController combatController;
        [SerializeField] private RewardController rewardController;

        [SerializeField] private GameObject panelRoot;
        [SerializeField] private List<RewardOption> rewardOptions = new List<RewardOption>();
        [SerializeField] private Button skipButton;

        private void Awake()
        {
            combatController.OnCombatEnded += HandleCombatEnded;

            skipButton.onClick.AddListener(SkipRewards);

            for (int i = 0; i < rewardOptions.Count; i++)
            {
                int index = i;
                rewardOptions[i].button.onClick.AddListener(() => SelectReward(index));
            }

            panelRoot.SetActive(false);
        }

        private void HandleCombatEnded(CombatState state)
        {
            if (state != CombatState.Victory)
                return;

            ShowRewards();
        }

        private void ShowRewards()
        {
            IReadOnlyList<CardData> rewards = rewardController.CurrentRewards;

            if (rewards.Count == 0)
            {
                Debug.LogWarning("RewardPanelUI: No rewards to display.");
                return;
            }

            for (int i = 0; i < rewardOptions.Count; i++)
            {
                RewardOption option = rewardOptions[i];

                if (i >= rewards.Count)
                {
                    option.button.gameObject.SetActive(false);
                    continue;
                }

                CardData cardData = rewards[i];

                option.button.gameObject.SetActive(true);
                option.cardName.text = cardData.Name;
                option.cardDescription.text = cardData.Description;

                option.cardArtwork.sprite = cardData.Sprite;
                option.cardArtwork.enabled = cardData.Sprite != null;
            }

            panelRoot.SetActive(true);
        }

        private void SelectReward(int index)
        {
            IReadOnlyList<CardData> rewards = rewardController.CurrentRewards;

            if (index < 0 || index >= rewards.Count)
                return;

            CardData selectedCard = rewards[index];

            if (rewardController.ClaimReward(selectedCard))
            {
                panelRoot.SetActive(false);
            }
        }

        private void SkipRewards()
        {
            rewardController.SkipRewards();
            panelRoot.SetActive(false);
        }

        private void OnDestroy()
        {
            combatController.OnCombatEnded -= HandleCombatEnded;
        }
    }
}
