using Gameplay.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EnemyHealthUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Enemy enemy;
        [SerializeField] private Image healthFill;
        
        private void OnEnable()
        {
            enemy.OnHealthChanged += HandleHealthChanged;
        }

        private void Start()
        {
            HandleHealthChanged(enemy.CurrentHealth);
        }

        private void HandleHealthChanged(int currentHealth)
        {
            float healthPercentage = (float)currentHealth / enemy.MaxHealth;
            healthFill.fillAmount = healthPercentage;
            healthText.text = $"{currentHealth}/{enemy.MaxHealth}";
            Debug.Log("Health Percentage: " + healthPercentage);
        }

        private void OnDisable()
        {
            enemy.OnHealthChanged -= HandleHealthChanged;
        }
    }
}
