using PixelAdventure.PlayerLogics;
using PixelAdventure.API;
using UnityEngine;

namespace PixelAdventure.Traps
{
    public class Fan : MonoBehaviour
    {
        private const float k_PUSH_FORCE_HORIZONTAL = 70f;
        private const float k_PUSH_FORCE_VERTICAL = 100f;
        private const float k_AIR_DRAG = 1.25f;

        [SerializeField] bool m_StartOn = true;
        [SerializeField] FanAirDirection m_AirDirection = FanAirDirection.Up;
        [SerializeField] float m_SwitchTime = 2f;

        private ParticleSystem m_Particles = null;
        private Animator m_Animator = null;

        private bool m_On = false;
        private float m_Timer = 0f;

        private void Start() 
        {
            m_On = m_StartOn;

            m_Particles = GetComponentInChildren<ParticleSystem>();
            m_Particles.Play();

            m_Animator = GetComponent<Animator>();
            m_Animator.SetBool(AnimatorHashes.ON, m_On);

            m_Timer = m_SwitchTime;
            if (m_On)
                m_Particles.Play();
            else
                m_Particles.Stop();
            m_Animator.SetBool(AnimatorHashes.ON, m_On);
        }

        private void Update() 
        {
            m_Timer -= Time.deltaTime;
            if (m_Timer <= 0f)
            {
                m_On = !m_On;
                m_Timer = m_SwitchTime;
                if (m_On)
                    m_Particles.Play();
                else
                    m_Particles.Stop();
                m_Animator.SetBool(AnimatorHashes.ON, m_On);
            }
        }

        private void Logics()
        {
            if (!m_On) return;

            PlayerMovement movement = Player.Movement;

            var rb = Player.Rigidbody2D;
            switch (m_AirDirection)
            {
                case FanAirDirection.Up:
                    movement.Grounded = false;
                    rb.AddForce(new Vector2(-rb.velocity.normalized.x * k_AIR_DRAG, k_PUSH_FORCE_VERTICAL));
                    break;
                case FanAirDirection.Down:
                    movement.Grounded = false;
                    rb.AddForce(new Vector2(-rb.velocity.normalized.x * k_AIR_DRAG, -k_PUSH_FORCE_VERTICAL));
                    break;
                case FanAirDirection.Left:
                    rb.AddForce(new Vector2(-k_PUSH_FORCE_HORIZONTAL, -rb.velocity.normalized.y * k_AIR_DRAG));
                    break;
                case FanAirDirection.Right:
                    rb.AddForce(new Vector2(k_PUSH_FORCE_HORIZONTAL, -rb.velocity.normalized.y * k_AIR_DRAG));
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag(Player.TAG)) Logics(); }
        private void OnTriggerStay2D(Collider2D other)  { if (other.CompareTag(Player.TAG)) Logics(); }
    }

    public enum FanAirDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}
