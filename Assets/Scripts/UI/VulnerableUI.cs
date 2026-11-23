using Gameplay.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class VulnerableUI : MonoBehaviour
    {
        [SerializeField] private Enemy enemy;
        [SerializeField] private Image vulnerableIcon;
        [SerializeField] private TMP_Text vulnerableValueText;

        private void OnEnable()
        {
            enemy.OnVulnerableChanged += HandleVulnerableChanged;
        }

        private void Start()
        {
            HandleVulnerableChanged(enemy.Vulnerable);
        }

        private void HandleVulnerableChanged(int value)
        {
            vulnerableIcon.gameObject.SetActive(value > 0);

            if (value > 0)
            {
                vulnerableValueText.text = value.ToString();
            }
        }

        private void OnDisable()
        {
            enemy.OnVulnerableChanged -= HandleVulnerableChanged;
        }
    }
}