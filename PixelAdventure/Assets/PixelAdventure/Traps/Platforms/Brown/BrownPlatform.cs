using PixelAdventure.Traps.Platforms.API;
using PixelAdventure.PlayerLogics;
using UnityEngine;

namespace PixelAdventure.Traps.Platforms.Brown
{
    public class BrownPlatform : MonoBehaviour
    {
        public const float SPEED = 3f;
        
        [SerializeField] Transform m_Target = null;
        [SerializeField] MovementType m_MovementType = MovementType.Vertical;

        private BaseMovement m_BaseMovement = null;
        private bool m_HasPlayer = false;

        private void Start() 
        {
            var body = GetComponent<Transform>();
            switch (m_MovementType)
            {
                case MovementType.Horizontal:
                    m_BaseMovement = new BrownHorizontalMovement
                    (
                        body      : body,
                        target    : m_Target.position.x,
                        origin    : body.position.x,
                        behaviour : this,
                        animator  : GetComponent<Animator>(),
                        hasPlayer : () => m_HasPlayer
                    );
                    break;
                case MovementType.Vertical:
                    m_BaseMovement = new BrownVerticalMovement
                    (
                        body      : body,
                        target    : m_Target.position.y,
                        origin    : body.position.y,
                        behaviour : this,
                        animator  : GetComponent<Animator>(),
                        hasPlayer : () => m_HasPlayer
                    );
                    break;
                default:
                    Debug.LogError("Unsupported movement type for brown platform!");
                    break;
            }   

            Destroy(m_Target.gameObject); 
        }

        private void FixedUpdate() => m_BaseMovement.Move();

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(Player.TAG))
            {
                other.transform.SetParent(this.transform);
                m_HasPlayer = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(Player.TAG))
            {
                other.transform.SetParent(null);
                m_HasPlayer = false;
            }
        }

    }
}