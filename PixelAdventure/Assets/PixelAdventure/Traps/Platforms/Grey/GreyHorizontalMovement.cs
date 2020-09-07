using PixelAdventure.Traps.Platforms.API;
using PixelAdventure.PlayerLogics;
using System.Collections;
using System;
using UnityEngine;

namespace PixelAdventure.Traps.Platforms.Grey
{
    public class GreyHorizontalMovement : BaseMovement
    {
        private static readonly int k_ON_HASH = Animator.StringToHash("on");
        private MonoBehaviour m_Behaviour = null;
        private GreyHorizontalBounds m_Bounds;
        private Transform m_Body = null;
        private bool m_MoveLock = false;
        private bool m_MoveLeft = true;
        private Animator m_Animator;
        private Func<bool> m_HasPlayer;

        public GreyHorizontalMovement(Transform body, float left, float right, MonoBehaviour behaviour, Animator animator, Func<bool> hasPlayer) 
        {
            m_Body = body;
            m_Bounds = new GreyHorizontalBounds(left, right);
            m_Behaviour = behaviour;
            m_Animator = animator;
            m_HasPlayer = hasPlayer;

            m_Animator.SetBool(k_ON_HASH, true);
        }

        public override void Move()
        {
            if (!m_MoveLock)
                if (m_MoveLeft)
                    m_Behaviour.StartCoroutine(MoveTowards(m_Bounds.left));
                else
                    m_Behaviour.StartCoroutine(MoveTowards(m_Bounds.right));
            
        }

        private IEnumerator MoveTowards(float x)
        {
            m_MoveLock = true;

            if (m_Body.position.x > x)
                while (m_Body.position.x > x)
                {
                    m_Body.Translate
                    (
                        x: -GreyPlatform.SPEED * Time.deltaTime,
                        y: 0f,
                        z: 0f
                    );
                    yield return null;
                }
            else while (m_Body.position.x < x)
                {
                    m_Body.Translate
                    (
                        x: GreyPlatform.SPEED * Time.deltaTime,
                        y: 0f,
                        z: 0f
                    );
                    yield return null;
                }

            m_Animator.SetBool(k_ON_HASH, false);
            yield return new WaitForSeconds(GreyPlatform.OFF_TIME);
            m_Animator.SetBool(k_ON_HASH, true);

            m_MoveLock = false;
            m_MoveLeft = !m_MoveLeft;

            if (m_HasPlayer())
            {
                Player.Instance.gameObject.transform.SetParent(null);
                Vector3 scale = m_Body.localScale;
                scale.x *= -1;
                m_Body.localScale = scale;
                Player.Instance.gameObject.transform.SetParent(m_Body);
            }
            else
            {
                Vector3 scale = m_Body.localScale;
                scale.x *= -1;
                m_Body.localScale = scale;
            }
            
            yield return 0;
        }
    }
}