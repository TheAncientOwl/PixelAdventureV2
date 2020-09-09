using PixelAdventure.API;
using UnityEngine;

namespace PixelAdventure.PlayerLogics
{
    public class PlayerMovement : MonoBehaviour
    {
        #region << constants >>
        private static readonly Vector2 k_JUMP_VELOCITY         = new Vector2(0f, 24f);
        private static readonly Vector2 k_WALL_JUMP_VELOCITY    = new Vector2(6f, 21f);
        private static readonly Vector2 k_DOUBLE_JUMP_VELOCITY  = new Vector2(0f, 21.6f);
        private static readonly Vector2 k_WALL_SLIDING_VELOCITY = new Vector2(0f, -1.26f);

        private static readonly Vector2 k_GROUND_BOX_CAST_SIZE = new Vector2(0.99f, 0.46f);
        private static readonly Vector2 k_WALL_BOX_CAST_SIZE = new Vector2(1.4f, 1.44f);

        private const string k_SET_WALL_JUMP_TO_FALSE_FUNC = "SetWallJumpToFalse";
        private const string k_HORIZONTAL_AXIS = "Horizontal";

        private const float k_MOVEMENT_SMOOTHING = 0.085f;
        private const float k_WALL_JUMP_TIME = 0.05f;
        private const float k_RUN_SPEED = 450f;

        private const float k_GROUNDED_BUFFER = 0.09f;
        private const float k_JUMP_BUFFER = 0.2f;
        #endregion//<< constants >> << ============================================================================================ >>
        #region << properties >>
        public bool Grounded { get { return m_Grounded; } set { m_Grounded = value; } }
        public float LastDirection => m_LastDirection;
        public float Direction => m_Direction;
        public bool CanMove { get { return m_CanMove; } set { m_CanMove = value; } }
        #endregion//<< properties >> << =========================================================================================== >>
        #region << serialized fields >>
        [SerializeField] bool m_FlipOnStart = false;
        [SerializeField] LayerMask m_GroundLayerMask = 0;
        [SerializeField] LayerMask m_WallLayerMask = 0;
        #endregion//<< serialized fields >> << ==================================================================================== >>
        #region << class variables >>
        private Vector2 m_AuxVelocity = Vector2.zero;
        private float m_LastDirection = 0f;
        private bool m_FacingRight = true;
        private float m_Direction = 0f;
        private bool m_CanMove = true;

        private float m_GroundedBuffer = 0f;
        private bool m_Grounded = false;

        private bool m_CanDoubleJump = false;
        private float m_JumpBuffer = 0f;

        private bool m_WallSliding = false;
        private bool m_WallJump = false;
        #endregion//<< class variables >> << ====================================================================================== >>

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

            m_GroundedBuffer = m_Grounded ? k_GROUNDED_BUFFER : m_GroundedBuffer - Time.deltaTime;

            if (m_Grounded || Player.YVelocity > 0f) 
                m_WallSliding = false;

            m_JumpBuffer = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) ? k_JUMP_BUFFER : m_JumpBuffer - Time.deltaTime;

            if (m_WallJump) 
                Player.Velocity = new Vector2(k_WALL_JUMP_VELOCITY.x * -m_LastDirection, k_WALL_JUMP_VELOCITY.y);
            else if (m_CanMove && m_WallSliding) 
                Player.Velocity = k_WALL_SLIDING_VELOCITY;
               
            //Debug.Log(Player.Velocity);

            if (m_JumpBuffer > 0f)
            {
                m_JumpBuffer = 0f;

                if (m_WallSliding) WallJump();
                else if (m_GroundedBuffer > 0f) Jump();
                else if (m_CanDoubleJump) DoubleJump();
            }

            Player.Animator.SetBool(AnimatorHashes.GROUNDED, m_Grounded);
            Player.Animator.SetBool(AnimatorHashes.WALL_SLIDING, m_WallSliding);
            if (m_Grounded)
            {
                if (m_CanMove)
                    Player.Animator.SetBool(AnimatorHashes.RUN, m_Direction != 0);
                else
                    Player.Animator.SetBool(AnimatorHashes.RUN, false);
            }
            else
                Player.Animator.SetFloat(AnimatorHashes.Y_VELOCITY, Player.YVelocity);
        }

        private void FixedUpdate()
        {
            Bounds bounds = Player.BoxCollider2D.bounds;

            if (Player.YVelocity < 1f)
                GroundBoxCast(bounds.center, bounds.size.y);

            WallBoxCast(bounds.center);

            if (m_CanMove)
                MoveHorizontal(Time.fixedDeltaTime);
        }

        #region << box casts >>
        private void GroundBoxCast(Vector2 origin, float colliderHeight)
        {
            m_Grounded = Physics2D.BoxCast
            (
                origin    : origin - new Vector2(0f, colliderHeight / 2f),
                size      : k_GROUND_BOX_CAST_SIZE,
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
                    size      : k_WALL_BOX_CAST_SIZE,
                    layerMask : m_WallLayerMask,
                    direction : Vector2.left,
                    distance  : 0f,
                    angle     : 0f
                ).collider != null;
            }
        }
        #endregion//<< box casts >> << ============================================================================================ >>
        #region << jumps >>
        private void Jump()
        {
            m_Grounded = false;
            m_GroundedBuffer = 0f;
            m_CanDoubleJump = true;
            Player.Velocity = k_JUMP_VELOCITY;
        }

        private void DoubleJump()
        {
            m_CanDoubleJump = false;
            Player.Velocity = k_DOUBLE_JUMP_VELOCITY;
            Player.Animator.SetTrigger(AnimatorHashes.DOUBLE_JUMP);
        }

        private void WallJump()
        {
            if (m_WallJump) return;
            m_WallJump = true;
            m_WallSliding = false;
            m_CanDoubleJump = true;
            Player.Animator.SetBool(AnimatorHashes.WALL_SLIDING, false);
            Invoke(k_SET_WALL_JUMP_TO_FALSE_FUNC, k_WALL_JUMP_TIME);
        }
        private void SetWallJumpToFalse() => m_WallJump = false;
        #endregion//<< jumps >> << ================================================================================================ >>
        #region << movement >>
        private void MoveHorizontal(float deltaTime)
        {
            float move = k_RUN_SPEED * m_Direction * deltaTime;
            Player.Velocity = Vector2.SmoothDamp
            (
                current         : Player.Velocity,
                target          : new Vector2(move, Player.YVelocity),
                currentVelocity : ref m_AuxVelocity,
                smoothTime      : k_MOVEMENT_SMOOTHING
            );

            if ((move > 0f && !m_FacingRight) || (move < 0f && m_FacingRight))
            {
                m_FacingRight = !m_FacingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
        #endregion//<< movement >> << ============================================================================================= >>
        #region << gizmos >>
        [Header("Gizmos")]
        [SerializeField] Color m_GroundColor = Color.yellow;
        [SerializeField] Color m_WallColor = Color.blue;
        private void OnDrawGizmos() 
        {
            Vector3 center = GetComponent<BoxCollider2D>().bounds.center + Vector3.down * (GetComponent<BoxCollider2D>().bounds.size.y / 2f);
            Vector2 halfSize = k_GROUND_BOX_CAST_SIZE / 2f;
            DrawBox2D(center, halfSize, m_GroundColor);

            center = GetComponent<Transform>().position;
            halfSize = k_WALL_BOX_CAST_SIZE / 2f;
            DrawBox2D(center, halfSize, m_WallColor);
        }

        private void DrawBox2D(Vector3 center, Vector2 halfSize, Color color)
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
        #endregion//<< gizmos >> << =============================================================================================== >>

    }
}