using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Gameplay.Characters;

namespace UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Player player;
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
            player.OnHealthChanged += HandleHealthChanged;
            player.OnBlockChanged += HandleBlockChanged;
        }

        private void Start()
        {
            HandleHealthChanged(player.CurrentHealth);
            HandleBlockChanged(player.Block);
        }

        private void HandleHealthChanged(int currentHealth)
        {
            float healthPercentage = (float)currentHealth / player.MaxHealth;
            healthFill.fillAmount = healthPercentage;
            healthText.text = $"{currentHealth}/{player.MaxHealth}";
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
            player.OnHealthChanged -= HandleHealthChanged;
            player.OnBlockChanged -= HandleBlockChanged;
        }
    }
}