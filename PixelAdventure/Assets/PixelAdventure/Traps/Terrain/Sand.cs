using PixelAdventure.PlayerLogics;
using UnityEngine;

namespace PixelAdventure.Traps.Terrain
{
    public class Sand : MonoBehaviour
    {
        private static readonly Vector2 k_IN_SAND_SIZE = new Vector2(0f, 0.15f);
        private const float k_DRAG_FORCE = 57f;
        private const float k_TIMER = 0.1f;

        private bool m_InSand = false;
        private float m_Timer = 0f;

        private void Update() => m_Timer += Time.deltaTime;

        private void OnCollisionEnter2D(Collision2D other) 
        {
            if (!m_InSand)
            {
                Player.Rigidbody2D.velocity = Vector2.zero;
                m_InSand = true;
                Player.BoxCollider2D.size -= k_IN_SAND_SIZE;
                m_Timer = 0f;
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            Player.Rigidbody2D.AddForce(new Vector2(-Player.Movement.Direction * k_DRAG_FORCE, 0f));
        }

        private void OnCollisionExit2D(Collision2D other) 
        {
            if (m_InSand && m_Timer > k_TIMER)
            {
                m_InSand = false;
                Player.BoxCollider2D.size += k_IN_SAND_SIZE;
            }
        }

    }
}
