using System;
using System.Collections;
using UnityEngine;

namespace Gameplay.Animation
{
    public class EnemyAnimationController : MonoBehaviour
    {
        [Header("Attack Animation")]
        [SerializeField] private float forwardDuration = 0.3f;
        [SerializeField] private float impactPauseDuration = 0.15f;
        [SerializeField] private float returnDuration = 0.2f;
        [SerializeField] private float attackDistance = 1f;
        
        [Header("Movement Curves")]
        [SerializeField] private AnimationCurve forwardCurve =
            AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [SerializeField] private AnimationCurve returnCurve =
            AnimationCurve.EaseInOut(1f, 1f, 0f, 0f);

        public IEnumerator PlayAttackAnimation(
            Transform target,
            Action onImpact)
        {
            Vector3 startPosition = transform.localPosition;

            Vector3 worldDirection =
                (target.position - transform.position).normalized;

            Vector3 localDirection =
                transform.parent.InverseTransformDirection(worldDirection);

            Vector3 attackPosition =
                startPosition + localDirection * attackDistance;

            yield return MoveTo(
                    startPosition,
                    attackPosition,
                    forwardDuration,
                    forwardCurve
            );

            // Враг достиг точки удара.
            onImpact?.Invoke();

            yield return new WaitForSeconds(impactPauseDuration);

            yield return MoveTo(
                    attackPosition,
                    startPosition,
                    returnDuration,
                    returnCurve
            );
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
