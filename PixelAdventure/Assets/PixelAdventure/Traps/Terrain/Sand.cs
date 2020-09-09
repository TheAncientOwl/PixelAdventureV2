using PixelAdventure.PlayerLogics;
using UnityEngine;

namespace PixelAdventure.Traps.Terrain
{
    public class Sand : MonoBehaviour
    {
        private const float k_DRAG_FORCE = 65f;

        private PlayerMovement m_PlayerMovement = null;
        private SpriteRenderer m_PlayerSpriteRenderer = null;
        private Rigidbody2D m_PlayerRigidbody2D = null;

        private bool m_Lock = false;

        private void Start() 
        {
            m_PlayerMovement = Player.Movement;
            m_PlayerSpriteRenderer = m_PlayerMovement.gameObject.GetComponent<SpriteRenderer>();
            m_PlayerRigidbody2D = Player.Rigidbody2D;    
        }

        private void OnCollisionStay2D(Collision2D other) 
        {
            m_PlayerRigidbody2D.AddForce(new Vector2(-m_PlayerMovement.Direction * k_DRAG_FORCE, 0f));

            if (!m_Lock)
            {
                m_Lock = true;
            }
            
        }

        private void OnCollisionExit2D(Collision2D other) 
        {
            
        }
    }
}
