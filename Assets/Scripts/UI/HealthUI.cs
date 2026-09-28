using TMPro;
using UnityEngine;

using Gameplay.Characters;
using UnityEngine.UI;

namespace UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Player player;
        [SerializeField] private Image healthFill;
        
        private void OnEnable()
        {
            player.OnHealthChanged += HandleHealthChanged;
        }

        private void Start()
        {
            HandleHealthChanged(player.CurrentHealth);
        }

        private void HandleHealthChanged(int currentHealth)
        {
            float healthPercentage = (float)currentHealth / player.MaxHealth;
            healthFill.fillAmount = healthPercentage;
            healthText.text = $"{currentHealth}/{player.MaxHealth}";
            Debug.Log("Health Percentage: " + healthPercentage);
        }

        private void OnDisable()
        {
            player.OnHealthChanged -= HandleHealthChanged;
        }
    }
    
    
}
