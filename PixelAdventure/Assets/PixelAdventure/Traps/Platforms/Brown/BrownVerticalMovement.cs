using PixelAdventure.Traps.Platforms.API;
using System.Collections;
using System;
using UnityEngine;

namespace PixelAdventure.Traps.Platforms.Brown
{
    public class BrownVerticalMovement : BaseMovement
    {
        private static readonly int k_ON_HASH = Animator.StringToHash("on");
        private MonoBehaviour m_Behaviour = null;
        private BrownBounds m_Bounds;
        private Transform m_Body = null;
        private bool m_MoveLock = false;
        private bool m_MoveToTarget = false;
        private Animator m_Animator;
        private Func<bool> m_HasPlayer;

        public BrownVerticalMovement(Transform body, float target, float origin, MonoBehaviour behaviour, Animator animator, Func<bool> hasPlayer)
        {
            m_Body = body;
            m_Bounds = target > origin ? new BrownBounds(target, origin) :new BrownBounds(origin, target);
            m_Behaviour = behaviour;
            m_Animator = animator;
            m_HasPlayer = hasPlayer;

            m_Animator.SetBool(k_ON_HASH, false);
        }

        public override void Move()
        {
            if (!m_MoveLock)
            {
                m_Animator.SetBool(k_ON_HASH, true);
                if (m_MoveToTarget)
                    m_Behaviour.StartCoroutine(MoveToTarget());
                else
                    m_Behaviour.StartCoroutine(MoveToOrigin());
            }
        }

        private IEnumerator MoveToOrigin()
        {
            m_MoveLock = true;

            if (m_Body.position.y > m_Bounds.origin)
                while (m_Body.position.y > m_Bounds.origin && !m_HasPlayer())
                {
                    m_Body.Translate
                    (
                        x: 0f,
                        y: -BrownPlatform.SPEED * Time.deltaTime,
                        z: 0f
                    );
                    yield return null;
                }

            m_Animator.SetBool(k_ON_HASH, false);

            m_MoveLock = false;
            m_MoveToTarget = !m_MoveToTarget;

            yield return 0;
        }

        private IEnumerator MoveToTarget()
        {
            m_MoveLock = true;

            if (m_Body.position.y < m_Bounds.target)
                while (m_Body.position.y < m_Bounds.target && m_HasPlayer())
                {
                    m_Body.Translate
                    (
                        x: 0f,
                        y: BrownPlatform.SPEED * Time.deltaTime,
                        z: 0f
                    );
                    yield return null;
                }

            m_Animator.SetBool(k_ON_HASH, false);
            while (m_HasPlayer()) yield return null;

            m_MoveLock = false;
            m_MoveToTarget = !m_MoveToTarget;

            yield return 0;
        }
    }
}