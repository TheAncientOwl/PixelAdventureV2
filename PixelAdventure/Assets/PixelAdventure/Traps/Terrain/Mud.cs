using PixelAdventure.PlayerLogics;
using UnityEngine;

namespace PixelAdventure.Traps.Terrain
{
    public class Mud : MonoBehaviour
    {
        private static readonly Vector2 k_IN_SAND_SIZE = new Vector2(0f, 0.3f);
        private const float k_TIMER = 0.1f;

        private bool m_InMud = false;
        private float m_Timer = 0f;

        private void Update() => m_Timer += Time.deltaTime;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!m_InMud)
            {
                Debug.Log("Enter");
                Player.Velocity = Vector2.zero;
                m_InMud = true;
                Player.BoxCollider2D.size -= k_IN_SAND_SIZE;
                m_Timer = 0f;
                Player.MovementX.CanMove = false;
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            Player.MovementX.CanMove = false;
            if (Player.YVelocity <= 0.01f)
                Player.Velocity = Vector2.zero;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (m_InMud && m_Timer > k_TIMER)
            {
                Debug.Log("Exit");
                m_InMud = false;
                Player.BoxCollider2D.size += k_IN_SAND_SIZE;
                Player.MovementX.CanMove = true;
            }
        }
    }
}
