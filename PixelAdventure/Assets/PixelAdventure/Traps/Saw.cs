using System.Collections;
using UnityEngine;

namespace PixelAdventure.Traps
{
    public class Saw : MonoBehaviour
    {
        [SerializeField] float m_OffTime = 0.5f;
        [SerializeField] Transform[] m_Targets = null;

        private Transform m_Body = null;
        private Vector2[] m_Points = null;
        private int m_PointIndex = 0;


        private static float k_SPEED = 2f;
        private static readonly int k_ON_HASH = Animator.StringToHash("on");
        private Animator m_Animator = null;
        private bool m_MoveLock = false;

        private void Start() 
        {
            m_Points = new Vector2[m_Targets.Length];
            for (int i = 0; i < m_Targets.Length; ++i)
            {
                m_Points[i] = m_Targets[i].position;
                Destroy(m_Targets[i].gameObject);
            }

            m_Body = GetComponent<Transform>();
            m_Body.position = m_Points[0];
            m_PointIndex = 0;

            m_Animator = GetComponent<Animator>();
            m_Animator.SetBool(k_ON_HASH, true);
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

            if (m_OffTime > 0f) yield return new WaitForSeconds(m_OffTime);

            m_MoveLock = false;

            yield return 0;
        }
    }
}
