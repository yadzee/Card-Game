using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TurnNotificationUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text turnText;

        [Header("Animation")]
        [SerializeField] private float fadeInDuration = 0.25f;
        [SerializeField] private float displayDuration = 0.7f;
        [SerializeField] private float fadeOutDuration = 0.25f;

        private void Awake()
        {
            canvasGroup.alpha = 0f;
        }

        public IEnumerator ShowTurnNotification(string message)
        {
            turnText.text = message;

            yield return FadeTo(1f, fadeInDuration);

            yield return new WaitForSeconds(displayDuration);

            yield return FadeTo(0f, fadeOutDuration);
        }

        private IEnumerator FadeTo(float targetAlpha, float duration)
        {
            float startAlpha = canvasGroup.alpha;

            if (duration <= 0f)
            {
                canvasGroup.alpha = targetAlpha;
                yield break;
            }

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                float progress = Mathf.Clamp01(elapsedTime / duration);

                canvasGroup.alpha = Mathf.Lerp(
                    startAlpha,
                    targetAlpha,
                    progress
                );

                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
        }
    }
}