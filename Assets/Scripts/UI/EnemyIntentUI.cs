using Gameplay.Characters;
using Gameplay.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EnemyIntentUI : MonoBehaviour
    {
        [SerializeField] private Enemy enemy;
        [SerializeField] private Image intentIcon;
        [SerializeField] private TMP_Text intentValueText;
        [SerializeField] private Sprite attackSprite;
        [SerializeField] private Sprite blockSprite;
        [SerializeField] private Color attackColor;
        [SerializeField] private Color blockColor;

        private void OnEnable()
        {
            enemy.OnIntentChanged += HandleIntentChanged;
        }

        private void Start()
        {
            HandleIntentChanged(enemy.CurrentIntent, enemy.IntentValue);
        }

        private void HandleIntentChanged(EnemyIntent intent, int value)
        {
            switch (intent)
            {
                case EnemyIntent.Attack:
                    SetIntentIcon(attackSprite, attackColor);
                    break;
                case EnemyIntent.Block:
                    SetIntentIcon(blockSprite, blockColor);
                    break;
            }

            intentValueText.text = value.ToString();
        }

        private void SetIntentIcon(Sprite sprite, Color color)
        {
            intentIcon.sprite = sprite == null ? null : sprite;
            intentIcon.color = color;
        }

        private void OnDisable()
        {
            enemy.OnIntentChanged -= HandleIntentChanged;
        }
    }
}
