using System.Collections;
using Gameplay.Characters;
using Gameplay.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EnemyIntentUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Enemy enemy;
        [SerializeField] private Image intentIcon;
        [SerializeField] private TMP_Text intentValueText;
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Intent Sprites")]
        [SerializeField] private Sprite attackSprite;
        [SerializeField] private Sprite blockSprite;
        [SerializeField] private Color attackColor;
        [SerializeField] private Color blockColor;

        [Header("Fade Animation")]
        [SerializeField] private float fadeDuration = 0.25f;
        
        [Header("Idle Animation")]
        [SerializeField] private float idleAmplitude = 2f;
        [SerializeField] private float idleFrequency = 1.5f;
        
        [Header("Scale Pulse")]
        [SerializeField] private float pulseDuration = 0.2f;
        [SerializeField] private float scaleMultiplier = 1.15f;

        private Vector3 _startScale;

        private RectTransform _rectTransform;
        private Vector2 _startPosition;

        private Coroutine _fadeCoroutine;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _startPosition = _rectTransform.anchoredPosition;
            _startScale = _rectTransform.localScale;
            canvasGroup.alpha = 0f;
        }

        private void OnEnable()
        {
            enemy.OnIntentChanged += HandleIntentChanged;
        }

        private void Start()
        {
            HandleIntentChanged(enemy.CurrentIntent, enemy.IntentValue);
        }
        
        private void Update()
        {
            float offset = Mathf.Sin(
                Time.time * idleFrequency * Mathf.PI * 2f
            ) * idleAmplitude * canvasGroup.alpha;

            _rectTransform.anchoredPosition = _startPosition + Vector2.up * offset;
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

        public void PlayFadeIn()
        {
            StopFade();

            _fadeCoroutine = StartCoroutine(FadeInRoutine());
        }

        public IEnumerator FadeOut()
        {
            StopFade();

            yield return FadeTo(0f);
        }

        private IEnumerator FadeInRoutine()
        {
            yield return FadeTo(1f);

            _fadeCoroutine = null;
        }

        private IEnumerator FadeTo(float targetAlpha)
        {
            float startAlpha = canvasGroup.alpha;

            if (fadeDuration <= 0f)
            {
                canvasGroup.alpha = targetAlpha;
                yield break;
            }

            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;

                float progress = Mathf.Clamp01(
                    elapsedTime / fadeDuration
                );

                canvasGroup.alpha = Mathf.Lerp(
                    startAlpha,
                    targetAlpha,
                    progress
                );

                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
        }

        private void StopFade()
        {
            if (_fadeCoroutine == null)
                return;

            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }
        
        public IEnumerator PlayScalePulse()
        {
            if (pulseDuration <= 0f)
                yield break;

            float elapsedTime = 0f;

            while (elapsedTime < pulseDuration)
            {
                elapsedTime += Time.deltaTime;

                float progress = Mathf.Clamp01(
                    elapsedTime / pulseDuration
                );

                float curveValue = Mathf.Sin(progress * Mathf.PI);

                _rectTransform.localScale = Vector3.Lerp(
                    _startScale,
                    _startScale * scaleMultiplier,
                    curveValue
                );

                yield return null;
            }

            _rectTransform.localScale = _startScale;
        }

        private void OnDisable()
        {
            StopFade();
            enemy.OnIntentChanged -= HandleIntentChanged;
        }
    }
}
