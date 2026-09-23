using System.Linq;
using Gameplay.Cards;
using Gameplay.Cards.Effects;
using Gameplay.Characters;
using Gameplay.Relics;
using UI;
using UnityEngine;

namespace Gameplay.Combat
{
    public class CombatController : MonoBehaviour
    {
        private Turns _currentTurn;
        [SerializeField] private Player player;
        [SerializeField] private Enemy enemy;
        [SerializeField] private Deck.Deck deck;
        [SerializeField] private RumCup rumCup;
        [SerializeField] private PlayerHandUI playerHandUI;


        private void Start()
        {
            PlayerTurn();
        }

        private void Update()
        {
            // CombatController.Update()
            if (Input.GetKeyDown(KeyCode.B))
            {
                foreach (var card in deck.Hand)
                {
                    if (card.Name == "Bash")
                    {
                        PlayCard(card);
                        break;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                foreach (var card in deck.Hand)
                {
                    if (card.Name == "Strike")
                    {
                        PlayCard(card);
                        break;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                foreach (var card in deck.Hand)
                {
                    if (card.Name == "Defend")
                    {
                        PlayCard(card);
                        break;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                EndPlayerTurn();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                foreach (var card in deck.Hand)
                {
                    Debug.Log($"Hand card: {card.Name}");
                }

                foreach (var card in deck.Hand)
                {
                    if (card.Name == "Strike")
                    {
                        PlayCard(card);
                        break;
                    }
                }
            }
        }


        private void EndPlayerTurn()
        {
            if (_currentTurn == Turns.PlayerTurn)
            {
                deck.DiscardHand();
                rumCup.ResetAfterTurn();
                EnemyTurn();
            }
        }

        private void PlayerTurn()
        {
            // _endTurn = false;
            player.ResetEnergy();
            player.ResetBlock();
            deck.DrawCards(5);
            playerHandUI.AddCardToHand(deck.Hand[0]);
            _currentTurn = Turns.PlayerTurn;
            Debug.Log("Player turn " + _currentTurn);
        }

        private void EnemyTurn()
        {
            _currentTurn = Turns.EnemyTurn;
            Debug.Log("Enemy turn");
            EnemyAction action = enemy.ExecuteIntent();
            switch (action.Intent)
            {
                case EnemyIntent.Attack:
                    player.TakeDamage(action.Value);
                    break;

                case EnemyIntent.Block:
                    enemy.Block = action.Value;
                    Debug.Log($"Enemy blocked: {enemy.Block}");
                    break;
            }

            enemy.ReduceVulnerable();
            enemy.ChooseNextIntent();
            Debug.Log("Enemy turn End");
            PlayerTurn();
        }

        public void PlayCard(Card card)
        {
            Debug.Log("PlayCard called");
            if (deck.Hand.Contains(card))
            {
                Debug.Log("Card is in hand");
                Debug.Log($"Card cost: {card.EnergyCost}, Current energy: {player.CurrentEnergy}");
                if (card.EnergyCost <= player.CurrentEnergy)
                {
                    Debug.Log("Enough energy");
                    CardEffect chosenEffect = rumCup.ChooseCard(card);

                    player.SpendEnergy(card.EnergyCost);
                    foreach (var effect in card.CardEffects)
                    {
                        if (effect == chosenEffect)
                        {
                            if (effect is DamageEffect)
                                ((DamageEffect)effect).ExecuteMultiplier(player, enemy,
                                    rumCup.GetEffectMultiplier(effect));
                            else if (effect is BlockEffect)
                                ((BlockEffect)effect).ExecuteMultiplier(player, enemy,
                                    rumCup.GetEffectMultiplier(effect));
                            rumCup.ConsumeBonus();
                        }

                        else
                        {
                            effect.Execute(player, enemy);
                        }
                    }

                    deck.DiscardCard(card);
                    rumCup.RegisterCardPlayed();
                }
            }
        }
    }
}