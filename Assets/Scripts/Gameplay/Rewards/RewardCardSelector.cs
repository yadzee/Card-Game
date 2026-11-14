using System;
using Gameplay.Cards;
using UnityEngine;
using Tools.UI;

namespace Gameplay.Rewards
{
    [RequireComponent(typeof(IMouseInput))]
    public class RewardCardSelector : MonoBehaviour
    {
        public event Action<CardData> OnSelected;

        private IMouseInput _mouseInput;
        private CardData _cardData;

        private void Awake()
        {
            _mouseInput = GetComponent<IMouseInput>();
            _mouseInput.OnPointerClick += HandlePointerClick;
        }

        public void Initialize(CardData cardData)
        {
            _cardData = cardData;
        }

        private void HandlePointerClick(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (_cardData == null)
                return;

            OnSelected?.Invoke(_cardData);
        }

        private void OnDestroy()
        {
            if (_mouseInput != null)
                _mouseInput.OnPointerClick -= HandlePointerClick;
        }
    }
}