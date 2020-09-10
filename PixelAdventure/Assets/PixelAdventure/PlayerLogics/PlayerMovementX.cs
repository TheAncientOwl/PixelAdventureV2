using PixelAdventure.API;
using UnityEngine;

namespace PixelAdventure.PlayerLogics
{
    public class PlayerMovementX : MonoBehaviour
    {
        private const string k_HORIZONTAL_AXIS = "Horizontal";
        private const float k_MOVEMENT_SMOOTHING = 0.085f;
        private const float k_RUN_SPEED = 450f;

        public float LastDirection => m_LastDirection;
        public float Direction => m_Direction;
        public bool CanMove { get { return m_CanMove; } set { m_CanMove = value; } }

        [SerializeField] bool m_FlipOnStart = false;

        private Vector2 m_AuxVelocity = Vector2.zero;
        private float m_LastDirection = 0f;
        private bool m_FacingRight = true;
        private float m_Direction = 0f;
        private bool m_CanMove = true;

        private void Start()
        {
            if (m_FlipOnStart)
            {
                m_FacingRight = !m_FacingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }

        private void Update() 
        {
            if (m_Direction != 0f)
                m_LastDirection = m_Direction;

            m_Direction = Input.GetAxisRaw(k_HORIZONTAL_AXIS);  
        }

        private void FixedUpdate() 
        {
            if (m_CanMove)
            {
                float move = k_RUN_SPEED * m_Direction * Time.fixedDeltaTime;
                Player.Velocity = Vector2.SmoothDamp
                (
                    current         : Player.Velocity,
                    target          : new Vector2(move, Player.YVelocity),
                    currentVelocity : ref m_AuxVelocity,
                    smoothTime      : k_MOVEMENT_SMOOTHING
                );

                Player.Animator.SetBool(AnimatorHashes.RUN, Player.MovementX.Direction != 0);

                if ((move > 0f && !m_FacingRight) || (move < 0f && m_FacingRight))
                {
                    m_FacingRight = !m_FacingRight;
                    Vector3 scale = transform.localScale;
                    scale.x *= -1;
                    transform.localScale = scale;
                }
            }
            else
                Player.Animator.SetBool(AnimatorHashes.RUN, false);
        }


    }
}
