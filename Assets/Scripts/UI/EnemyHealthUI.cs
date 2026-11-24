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

        [Header("Block UI")]
        [SerializeField] private Image blockIcon;
        [SerializeField] private TMP_Text blockText;
        [SerializeField] private Color blockHealthColor = new Color(0.25f, 0.65f, 0.95f, 1f);

        private Color _normalHealthColor;

        private void Awake()
        {
            _normalHealthColor = healthFill.color;
        }

        private void OnEnable()
        {
            enemy.OnHealthChanged += HandleHealthChanged;
            enemy.OnBlockChanged += HandleBlockChanged;
        }

        private void Start()
        {
            HandleHealthChanged(enemy.CurrentHealth);
            HandleBlockChanged(enemy.Block);
        }

        private void HandleHealthChanged(int currentHealth)
        {
            float healthPercentage = (float)currentHealth / enemy.MaxHealth;
            healthFill.fillAmount = healthPercentage;
            healthText.text = $"{currentHealth}/{enemy.MaxHealth}";
        }

        private void HandleBlockChanged(int block)
        {
            bool hasBlock = block > 0;

            blockIcon.gameObject.SetActive(hasBlock);
            blockText.gameObject.SetActive(hasBlock);

            if (hasBlock)
                blockText.text = block.ToString();

            healthFill.color = hasBlock ? blockHealthColor : _normalHealthColor;
        }

        private void OnDisable()
        {
            enemy.OnHealthChanged -= HandleHealthChanged;
            enemy.OnBlockChanged -= HandleBlockChanged;
        }
    }
}