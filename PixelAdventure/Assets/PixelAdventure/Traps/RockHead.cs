using PixelAdventure.API;
using System.Collections;
using UnityEngine;

namespace PixelAdventure.Traps
{
    public class RockHead : MonoBehaviour
    {
        [SerializeField] Transform[] m_Targets = null;

        private static float k_BLINK_ANIMATION_TIME = 0.2f;
        private static float k_HIT_ANIMATION_TIME = 0.2f;
        private static float k_TIME_BEFORE_BLINK = 0.04f;
        private static float k_IDLE_TIME = 1f - 0.2f;//- k_HIT_ANIMATION_TIME
        private static float k_SPEED = 16f;

        private static readonly int k_IDLE_HASH   = Animator.StringToHash("idle");
        private static readonly int k_BLINK_HASH  = Animator.StringToHash("blink");
        private static readonly int k_TOP_HIT     = Animator.StringToHash("topHit");
        private static readonly int k_LEFT_HIT    = Animator.StringToHash("leftHit");
        private static readonly int k_RIGHT_HIT   = Animator.StringToHash("rightHit");
        private static readonly int k_BOTTOM_HIT  = Animator.StringToHash("bottomHit");

        private Animator m_Animator = null;
        private CameraShaker m_CameraShaker = null;
        private CircleCollider2D m_CircleCollider2D = null;

        private Vector2[] m_Points = null;
        private int[] m_AnimationHashes = null;
        private Transform m_Body = null;
        private bool m_MoveLock = false;
        private int m_PointIndex = 0;

        private void Start()
        {
            m_Points = new Vector2[m_Targets.Length];
            for (int i = 0; i < m_Targets.Length; ++i)
            {
                m_Points[i] = m_Targets[i].position;
                Destroy(m_Targets[i].gameObject);
            }

            m_AnimationHashes = new int[m_Points.Length];
            m_AnimationHashes[0] = GetAnimationTriggerHash
            (
                startPos: m_Points[m_Points.Length - 1],
                endPos: m_Points[0]
            );
            for (int i = 1; i < m_Points.Length; ++i)
                m_AnimationHashes[i] = GetAnimationTriggerHash
                (
                    startPos: m_Points[i - 1],
                    endPos: m_Points[i]
                );

            m_Body = GetComponent<Transform>();
            m_Body.position = m_Points[0];
            m_PointIndex = 0;

            m_Animator = GetComponent<Animator>();

            m_CircleCollider2D = GetComponent<CircleCollider2D>();

            m_CameraShaker = CameraShaker.Instance;
        }

        private void Update()
        {
            if (!m_MoveLock)
            {
                m_PointIndex = (m_PointIndex + 1) % m_Points.Length;
                StartCoroutine(MoveTowards(m_Points[m_PointIndex]));
            }
        }

        private IEnumerator MoveTowards(Vector2 pos)
        {
            m_MoveLock = true;
            m_CircleCollider2D.enabled = false;

            Invoke("Blink", k_TIME_BEFORE_BLINK);

            if (m_Body.position.x > pos.x)
                while (m_Body.position.x > pos.x)
                {
                    m_Body.Translate
                    (
                        x: -k_SPEED * Time.deltaTime,
                        y: 0f,
                        z: 0f
                    );
                    yield return null;
                }
            else if (m_Body.position.x < pos.x)
                while (m_Body.position.x < pos.x)
                {
                    m_Body.Translate
                    (
                        x: k_SPEED * Time.deltaTime,
                        y: 0f,
                        z: 0f
                    );
                    yield return null;
                }

            if (m_Body.position.y > pos.y)
                while (m_Body.position.y > pos.y)
                {
                    m_Body.Translate
                    (
                        x: 0f,
                        y: -k_SPEED * Time.deltaTime,
                        z: 0f
                    );
                    yield return null;
                }
            else if (m_Body.position.y < pos.y)
                while (m_Body.position.y < pos.y)
                {
                    m_Body.Translate
                    (
                        x: 0f,
                        y: k_SPEED * Time.deltaTime,
                        z: 0f
                    );
                    yield return null;
                }

            m_Animator.SetTrigger(m_AnimationHashes[m_PointIndex]);
            //m_CameraShaker.Shake();
            m_CircleCollider2D.enabled = true;
            yield return new WaitForSeconds(k_HIT_ANIMATION_TIME);

            m_Animator.SetTrigger(k_IDLE_HASH);
            yield return new WaitForSeconds(k_IDLE_TIME);

            m_MoveLock = false;

            yield return 0;
        }

        private void Blink()
        {
            m_Animator.SetTrigger(k_BLINK_HASH);
            Invoke("Idle", k_BLINK_ANIMATION_TIME);
        }

        private void Idle() => m_Animator.SetTrigger(k_IDLE_HASH);

        private int GetAnimationTriggerHash(Vector2 startPos, Vector2 endPos)
        {
            if (startPos.x > endPos.x)
                return k_LEFT_HIT;

            if (startPos.x < endPos.x)
                return k_RIGHT_HIT;

            if (startPos.y > endPos.y)
                return k_BOTTOM_HIT;

            if (startPos.y < endPos.y)
                return k_TOP_HIT;

            Debug.LogError("Something went wrong in");
            return k_IDLE_HASH;
        }
    }
}
