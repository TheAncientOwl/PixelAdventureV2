using PixelAdventure.API;
using System.Collections;
using UnityEngine;

namespace PixelAdventure.Traps
{
    public class RockHead : MonoBehaviour
    {
        private const float k_IDLE_TIME = 1f - 0.2f;//- k_HIT_ANIMATION_TIME
        private const float k_BLINK_ANIMATION_TIME = 0.2f;
        private const float k_HIT_ANIMATION_TIME = 0.2f;
        private const float k_TIME_BEFORE_BLINK = 0.04f;
        private const float k_SPEED = 16f;

        [SerializeField] Transform[] m_Targets = null;

        private CircleCollider2D m_CircleCollider2D = null;
        private CameraShaker m_CameraShaker = null;
        private Animator m_Animator = null;
        private Transform m_Body = null;

        private int[] m_AnimationHashes = null;
        private Vector2[] m_Points = null;
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
            m_CameraShaker.Shake();
            m_CircleCollider2D.enabled = true;
            yield return new WaitForSeconds(k_HIT_ANIMATION_TIME);

            m_Animator.SetTrigger(AnimatorHashes.IDLE);
            yield return new WaitForSeconds(k_IDLE_TIME);

            m_MoveLock = false;

            yield return 0;
        }

        private void Blink()
        {
            m_Animator.SetTrigger(AnimatorHashes.BLINK);
            Invoke("Idle", k_BLINK_ANIMATION_TIME);
        }

        private void Idle() => m_Animator.SetTrigger(AnimatorHashes.IDLE);

        private int GetAnimationTriggerHash(Vector2 startPos, Vector2 endPos)
        {
            if (startPos.x > endPos.x)
                return AnimatorHashes.LEFT_HIT;

            if (startPos.x < endPos.x)
                return AnimatorHashes.RIGHT_HIT;

            if (startPos.y > endPos.y)
                return AnimatorHashes.BOTTOM_HIT;

            if (startPos.y < endPos.y)
                return AnimatorHashes.TOP_HIT;

            Debug.LogError("Something went wrong in");
            return AnimatorHashes.IDLE;
        }
    }
}
