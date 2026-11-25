using System.Collections;
using UnityEngine;

namespace Gameplay.Animation
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [Header("Hit Reaction")]
        [SerializeField] private float knockbackDistance = 0.3f;
        [SerializeField] private float knockbackDuration = 0.15f;
        [SerializeField] private float returnDuration = 0.2f;

        [Header("Movement Curves")]
        [SerializeField] private AnimationCurve knockbackCurve =
            AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [SerializeField] private AnimationCurve returnCurve =
            AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        private Vector3 _startPosition;

        private void Awake()
        {
            _startPosition = transform.localPosition;
        }

        public void PlayHitReaction(Transform attacker)
        {
            StartCoroutine(HitReaction(attacker));
        }

        private IEnumerator HitReaction(Transform attacker)
        {
            Vector3 worldDirection =
                (transform.position - attacker.position).normalized;

            Vector3 localDirection =
                transform.parent.InverseTransformDirection(worldDirection);

            Vector3 knockbackPosition =
                _startPosition + localDirection * knockbackDistance;

            yield return MoveTo(
                _startPosition,
                knockbackPosition,
                knockbackDuration,
                knockbackCurve
            );

            yield return MoveTo(
                knockbackPosition,
                _startPosition,
                returnDuration,
                returnCurve
            );

            transform.localPosition = _startPosition;
        }

        private IEnumerator MoveTo(
            Vector3 startPosition,
            Vector3 targetPosition,
            float duration,
            AnimationCurve curve)
        {
            if (duration <= 0f)
            {
                transform.localPosition = targetPosition;
                yield break;
            }

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                float progress = Mathf.Clamp01(elapsedTime / duration);
                float curveValue = curve.Evaluate(progress);

                transform.localPosition = Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    curveValue
                );

                yield return null;
            }

            transform.localPosition = targetPosition;
        }
    }
}