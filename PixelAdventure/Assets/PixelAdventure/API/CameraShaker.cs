using System.Collections;
using UnityEngine;

namespace PixelAdventure.API
{
    public class CameraShaker : MonoBehaviour
    {
        public static CameraShaker Instance {get; private set;}
        private void Awake() => Instance = this;

        const float k_DURATION = 0.045f;
        const float k_MAGNITUDE = 0.075f;

        private bool m_Shake = false;

        public void Shake() => StartCoroutine(ShakeCoroutine());

        private IEnumerator ShakeCoroutine()
        {
            if (m_Shake) yield return 0;
            m_Shake = true;

            Vector3 originalPosition = transform.localPosition;

            float timer = 0f;

            while (timer < k_DURATION)
            {
                timer += Time.deltaTime;
                transform.localPosition += new Vector3
                (
                    x: Random.Range(-1f, 1f) * k_MAGNITUDE,
                    y: Random.Range(-1f, 1f) * k_MAGNITUDE,
                    z: originalPosition.z
                );

                yield return null;
            }

            transform.localPosition = originalPosition;
        }
    }
}