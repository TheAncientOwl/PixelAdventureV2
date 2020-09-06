using UnityEngine;

namespace PixelAdventure.PlayerLogics
{
    public class PlayerMovement : MonoBehaviour
    {
        public static PlayerMovement Instance {get; private set; }
        private void Awake() => Instance = this;
        public float YVelocity { get{ return m_Rigidbody2D.velocity.y;} private set{}}
        public Vector2 Velocity { get {return m_Rigidbody2D.velocity;} set{ m_Rigidbody2D.velocity = value; }}
        public bool IsGrounded { get{return m_Grounded;} set{m_Grounded = value;}}
        public Rigidbody2D Rigidbody2D { get{return m_Rigidbody2D;} private set{}}
        public float LastDirection { get{return m_LastDirection;} private set{}}

        private Rigidbody2D m_Rigidbody2D = null;
        private BoxCollider2D m_BoxCollider2D = null;

        private Animator m_Animator = null;
        private static readonly int k_RUN_HASH          = Animator.StringToHash("isRunning");
        private static readonly int k_GROUNDED_HASH     = Animator.StringToHash("isGrounded");
        private static readonly int k_Y_VELOCITY_HASH   = Animator.StringToHash("yVelocity");
        private static readonly int k_DOUBLE_JUMP_HASH  = Animator.StringToHash("isDoubleJumping");
        private static readonly int k_WALL_SLIDING_HASH = Animator.StringToHash("wallSliding");

        private static readonly string k_HORIZONTAL = "Horizontal";
        private static readonly string k_SET_WALL_JUMP_TO_FALSE = "SetWallJumpToFalse";
        private void SetWallJumpToFalse() => m_WallJump = false;

        [Header("Horizontal Movement")]
        [Range(0, 1)]
        [SerializeField] float m_MovementSmoothing = .05f;
        [SerializeField] float m_RunSpeed = 390f;

        [Header("Jump")]
        [SerializeField] Vector2 m_JumpVelocity = new Vector2(0f, 24f);
        [SerializeField] Vector2 m_DoubleJumpVelocity = new Vector2(0f, 21.6f);
        [SerializeField] Vector2 m_WallJumpVelocity = new Vector2(6f, 21f);
        [SerializeField] float m_WallJumpTime = 0.05f;

        [Header("On Wall")]
        [SerializeField] Vector2 m_WallSlidingVelocity = new Vector2(0f, -3f);

        [Header("Misc")]
        [SerializeField] bool m_FlipOnStart = false;
        [SerializeField] LayerMask m_GroundLayerMask = 0;
        [SerializeField] LayerMask m_WallLayerMask = 0;
        [SerializeField] Vector2 m_GroundBoxCastSize = new Vector2(1.264555f, 0.46f);
        [SerializeField] Vector2 m_WallBoxCastSize = new Vector2(1.45f, 0f);

        private Vector2 m_AuxVelocity = Vector2.zero;
        private bool m_FacingRight = true;
        private float m_Direction = 0f;

        private const float k_GROUNDED_BUFFER = 0.09f;
        private float m_GroundedBuffer = 0f;
        private bool m_Grounded = false;

        private const float k_JUMP_BUFFER = 0.2f;
        private bool m_CanDoubleJump = false;
        private float m_JumpBuffer = 0f;

        private bool m_WallSliding = false;
        private bool m_WallJump = false;
        private float m_LastDirection = 0f;

        private void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_BoxCollider2D = GetComponent<BoxCollider2D>();

            m_GroundBoxCastSize.x = m_BoxCollider2D.bounds.size.x - 0.2f;

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
            if (m_Direction != 0f) m_LastDirection = m_Direction;
            m_Direction = Input.GetAxisRaw(k_HORIZONTAL);

            m_GroundedBuffer = m_Grounded ? k_GROUNDED_BUFFER : m_GroundedBuffer - Time.deltaTime;
            if (m_Grounded || m_Rigidbody2D.velocity.y > 0f) m_WallSliding = false;

            m_JumpBuffer = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) ? k_JUMP_BUFFER : m_JumpBuffer - Time.deltaTime;

            if (m_WallJump) m_Rigidbody2D.velocity = new Vector2(m_WallJumpVelocity.x * -m_LastDirection, m_WallJumpVelocity.y);
            else if (m_WallSliding) m_Rigidbody2D.velocity = m_WallSlidingVelocity;

            if (m_JumpBuffer > 0f)
            {
                m_JumpBuffer = 0f;

                if (m_WallSliding) WallJump();
                else if (m_GroundedBuffer > 0f) Jump();
                else if (m_CanDoubleJump) DoubleJump();
            }

            m_Animator.SetBool(k_GROUNDED_HASH, m_Grounded);
            m_Animator.SetBool(k_WALL_SLIDING_HASH, m_WallSliding);
            if (m_Grounded) m_Animator.SetBool(k_RUN_HASH, m_Direction != 0);
            else m_Animator.SetFloat(k_Y_VELOCITY_HASH, m_Rigidbody2D.velocity.y);
        }

        private void FixedUpdate()
        {
            Bounds bounds = m_BoxCollider2D.bounds;
            if (m_Rigidbody2D.velocity.y < 1f)
                GroundBoxCast(bounds.center, bounds.size.y);
            WallBoxCast(bounds.center);

            MoveHorizontal(Time.fixedDeltaTime);
        }

        private void GroundBoxCast(Vector2 origin, float colliderHeight)
        {
            m_Grounded = Physics2D.BoxCast
            (
                origin    : origin - new Vector2(0f, colliderHeight / 2f),
                size      : m_GroundBoxCastSize,
                layerMask : m_GroundLayerMask,
                direction : Vector2.down,
                distance  : 0f,
                angle     : 0f
            ).collider != null;
        }

        private void WallBoxCast(Vector2 origin)
        {
            if (m_Grounded) 
               m_WallSliding = false;
            else
            {
                m_WallSliding = Physics2D.BoxCast
                (
                    origin    : origin,
                    size      : m_WallBoxCastSize,
                    layerMask : m_WallLayerMask,
                    direction : Vector2.left,
                    distance  : 0f,
                    angle     : 0f
                ).collider != null;
            }
        }

        private void Jump()
        {
            m_Grounded = false;
            m_GroundedBuffer = 0f;
            m_CanDoubleJump = true;
            m_Rigidbody2D.velocity = m_JumpVelocity;
        }

        private void DoubleJump()
        {
            m_CanDoubleJump = false;
            m_Rigidbody2D.velocity = m_DoubleJumpVelocity;
            m_Animator.SetTrigger(k_DOUBLE_JUMP_HASH);
        }

        private void WallJump()
        {
            if (m_WallJump) return;
            m_WallJump = true;
            m_WallSliding = false;
            m_CanDoubleJump = true;
            m_Animator.SetBool(k_WALL_SLIDING_HASH, false);
            Invoke(k_SET_WALL_JUMP_TO_FALSE, m_WallJumpTime);
        }

        private void MoveHorizontal(float deltaTime)
        {
            float move = m_RunSpeed * m_Direction * deltaTime;
            m_Rigidbody2D.velocity = Vector2.SmoothDamp
            (
                current         : m_Rigidbody2D.velocity,
                target          : new Vector2(move, m_Rigidbody2D.velocity.y),
                currentVelocity : ref m_AuxVelocity,
                smoothTime      : m_MovementSmoothing
            );

            if ((move > 0f && !m_FacingRight) || (move < 0f && m_FacingRight))
            {
                m_FacingRight = !m_FacingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }

        [Header("Gizmos")]
        [SerializeField] Color m_GroundColor = Color.yellow;
        [SerializeField] Color m_WallColor = Color.blue;
        private void OnDrawGizmos() 
        {
            Vector3 center = GetComponent<BoxCollider2D>().bounds.center + Vector3.down * (GetComponent<BoxCollider2D>().bounds.size.y / 2f);
            Vector2 halfSize = m_GroundBoxCastSize / 2f;
            Draw(center, halfSize, m_GroundColor);

            center = GetComponent<Transform>().position;
            halfSize = m_WallBoxCastSize / 2f;
            Draw(center, halfSize, m_WallColor);
        }

        private void Draw(Vector3 center, Vector2 halfSize, Color color)
        {
            Vector3 topLeft = new Vector3
            (
                x: center.x - halfSize.x,
                y: center.y + halfSize.y,
                z: center.z
            );

            Vector3 topRight = new Vector3
            (
                x: center.x + halfSize.x,
                y: center.y + halfSize.y,
                z: center.z
            );

            Vector3 bottomLeft = new Vector3
            (
                x: center.x - halfSize.x,
                y: center.y - halfSize.y,
                z: center.z
            );

            Vector3 bottomRight = new Vector3
            (
                x: center.x + halfSize.x,
                y: center.y - halfSize.y,
                z: center.z
            );

            Debug.DrawLine(topLeft, topRight, color, 0f);
            Debug.DrawLine(topRight, bottomRight, color, 0f);
            Debug.DrawLine(bottomRight, bottomLeft, color, 0f);
            Debug.DrawLine(bottomLeft, topLeft, color, 0f);
        }

    }
}