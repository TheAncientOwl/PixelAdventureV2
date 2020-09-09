using PixelAdventure.API;
using UnityEngine;

namespace PixelAdventure.PlayerLogics
{
    public class Player : MonoBehaviour
    {
        private static readonly int k_HIT_HASH = Animator.StringToHash("hit");

        public static Player Instance {get; private set;}
        public const string TAG = "Player";

        private static SpriteRenderer m_SpriteRenderer = null;
        private static BoxCollider2D m_BoxCollider2D = null;
        private static PlayerMovement m_Movement = null;
        private static Rigidbody2D m_Rigidbody2D = null;
        private static Transform m_Transform = null;
        private static Animator m_Animator = null;

        public static SpriteRenderer SpriteRenderer => m_SpriteRenderer;
        public static BoxCollider2D BoxCollider2D => m_BoxCollider2D;
        public static PlayerMovement Movement => m_Movement;
        public static Rigidbody2D Rigidbody2D => m_Rigidbody2D;
        public static Transform Transform => m_Transform;
        public static Animator Animator => m_Animator;

        public static Vector2 Velocity { get { return m_Rigidbody2D.velocity; } set { m_Rigidbody2D.velocity = value; } }
        public static float YVelocity => Velocity.y;

        private void Awake() 
        {
            Instance = this; 
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_BoxCollider2D = GetComponent<BoxCollider2D>();
            m_Movement = GetComponent<PlayerMovement>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Transform = GetComponent<Transform>();
            m_Animator = GetComponent<Animator>();
        }

        public void Die()
        {
            CameraShaker.Instance.Shake();
            GetComponent<Animator>().SetTrigger(k_HIT_HASH);
            GetComponent<BoxCollider2D>().enabled = false;
            Rigidbody2D rigidbody2D = Player.m_Rigidbody2D;
            rigidbody2D.constraints = RigidbodyConstraints2D.None;
            rigidbody2D.AddTorque(-Player.Movement.LastDirection * 5f, ForceMode2D.Impulse);
            rigidbody2D.velocity = new Vector2
            (
                x: -Player.Movement.LastDirection * 10f,
                y: 10f
            );
            GetComponent<PlayerMovement>().enabled = false;
        }
    }
}