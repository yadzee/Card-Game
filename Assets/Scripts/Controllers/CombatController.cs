using System.Linq;
using UnityEngine;

public class CombatController: MonoBehaviour
{
     private Turns _curretTurn;
     [SerializeField] private Player player;
     [SerializeField] private Enemy enemy;
     [SerializeField] private Deck deck;
     
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
    
 
    public void EndPlayerTurn()
    {
        if (_curretTurn == Turns.PlayerTurn)
        {
            deck.DiscardHand();
            EnemyTurn();
        }
    }

    public void PlayerTurn()
    {
        // _endTurn = false;
        player.ResetEnergy();
        player.ResetBlock();
        deck.DrawCards(5);
        _curretTurn = Turns.PlayerTurn;
        Debug.Log("Player turn " + _curretTurn);
    }
    
    public void EnemyTurn()
    {
        _curretTurn = Turns.EnemyTurn;
        Debug.Log("Enemy turn");
        EnemyAction action = enemy.ExecuteIntent();
        switch (action.Intent)
        {
            case EnemyIntent.Attack: 
                 player.ReceiveAttack(action);
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

                player.SpendEnergy(card.EnergyCost);
                foreach (var effect in card.CardEffects)
                {
                    effect.Execute(player,enemy);
                }
                deck.DiscardCard(card);
            }
        }
    }
    
    private enum Turns
    {
        PlayerTurn,
        EnemyTurn
    }
}
