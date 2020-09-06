using PixelAdventure.Traps.Platforms.API;
using System.Collections;
using UnityEngine;

namespace PixelAdventure.Traps.Platforms.Grey
{
    public class GreyVerticalMovement : BaseMovement
    {
        private static readonly int k_ON_HASH = Animator.StringToHash("on");
        private MonoBehaviour m_Behaviour = null;
        private GreyVerticalBounds m_Bounds;
        private Transform m_Body = null;
        private bool m_MoveLock = false;
        private bool m_MoveTop = true;
        private Animator m_Animator;

        public GreyVerticalMovement(Transform body, float top, float bottom, MonoBehaviour behaviour, Animator animator)
        {
            m_Body = body;
            m_Bounds = new GreyVerticalBounds(top, bottom);
            m_Behaviour = behaviour;
            m_Animator = animator;

            m_Animator.SetBool(k_ON_HASH, true);
        }

        public override void Move()
        {
            if (!m_MoveLock)
                if (m_MoveTop)
                    m_Behaviour.StartCoroutine(MoveTowards(m_Bounds.top));
                else
                    m_Behaviour.StartCoroutine(MoveTowards(m_Bounds.bottom));
        }

        private IEnumerator MoveTowards(float y)
        {
            m_MoveLock = true;

            if (m_Body.position.y > y)
                while (m_Body.position.y > y)
                {
                    m_Body.Translate
                    (
                        x: 0f,
                        y: -GreyPlatform.k_SPEED * Time.deltaTime,
                        z: 0f
                    );
                    yield return null;
                }
            else while (m_Body.position.y < y)
                {
                    m_Body.Translate
                    (
                        x: 0f,
                        y: GreyPlatform.k_SPEED * Time.deltaTime,
                        z: 0f
                    );
                    yield return null;
                }

            m_Animator.SetBool(k_ON_HASH, false);
            yield return new WaitForSeconds(GreyPlatform.k_OFF_TIME);
            m_Animator.SetBool(k_ON_HASH, true);

            m_MoveLock = false;
            m_MoveTop = !m_MoveTop;

            yield return 0;
        }
    }
}