using TMPro;
using UnityEngine;
using Gameplay.Characters;

namespace UI
{
    public class EnergyUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text energyText;
        [SerializeField] private Player player;
        
        private void OnEnable()
        {
            player.OnEnergyChanged += HandleEnergyChanged;
        }

        private void Start()
        {
            HandleEnergyChanged(player.CurrentEnergy);
        }

        private void HandleEnergyChanged(int currentEnergy)
        {
            energyText.text = currentEnergy.ToString();
        }

        private void OnDisable()
        {
            player.OnEnergyChanged -= HandleEnergyChanged;
        }
    }
    
}