﻿using PixelAdventure.Traps.Platforms.API;
using PixelAdventure.API;
using System.Collections;
using UnityEngine;

namespace PixelAdventure.Traps.Platforms.Grey
{
    public class GreyVerticalMovement : BaseMovement
    {
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

            m_Animator.SetBool(AnimatorHashes.ON, true);
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
                        y: -GreyPlatform.SPEED * Time.deltaTime,
                        z: 0f
                    );
                    yield return null;
                }
            else while (m_Body.position.y < y)
                {
                    m_Body.Translate
                    (
                        x: 0f,
                        y: GreyPlatform.SPEED * Time.deltaTime,
                        z: 0f
                    );
                    yield return null;
                }

            m_Animator.SetBool(AnimatorHashes.ON, false);
            yield return new WaitForSeconds(GreyPlatform.OFF_TIME);
            m_Animator.SetBool(AnimatorHashes.ON, true);

            m_MoveLock = false;
            m_MoveTop = !m_MoveTop;

            yield return 0;
        }
    }
}