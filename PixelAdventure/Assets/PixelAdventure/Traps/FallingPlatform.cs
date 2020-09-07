using PixelAdventure.PlayerLogics;
using System.Collections;
using UnityEngine;

namespace PixelAdventure.Traps
{
    public class FallingPlatform : MonoBehaviour
    {
        private static readonly int k_OFF_HASH = Animator.StringToHash("off");
        private static readonly float k_UP_DISTANCE = 0.25f;
        private static readonly float k_SPEED = 0.3f;

        private bool m_MoveToTarget = true;
        private bool m_MoveLock = false;
        private Transform m_Body = null;
        private float m_Target = 0f;
        private float m_Origin = 0f;

        private bool m_On = true;
        private bool m_Falling = false;

        private void Start() 
        {
            GetComponent<Rigidbody2D>().isKinematic = true;

            m_Body = GetComponent<Transform>();
            m_Origin = m_Body.position.y;
            m_Target = m_Body.position.y + k_UP_DISTANCE;
        }

        // private void OnCollisionEnter2D(Collision2D other)
        // {
        //     if (other.gameObject.CompareTag(Player.TAG))
        //     {
        //         Debug.Log(PlayerMovement.Instance.YVelocity + " | Grounded: " + PlayerMovement.Instance.IsGrounded);
        //     }
        //     if (other.gameObject.CompareTag(Player.TAG) && PlayerMovement.Instance.IsGrounded)
        //         Invoke("Fall", 0.6f);
        // }

        private void OnCollisionStay2D(Collision2D other) 
        {
            if (other.gameObject.CompareTag(Player.TAG) && PlayerMovement.Instance.Grounded && !m_Falling)
                Invoke("Fall", 0.5f);
        }

        private void Fall()
        {
            m_Falling = true;
            m_On = false;
            GetComponent<Animator>().SetTrigger(k_OFF_HASH);
            GetComponent<Rigidbody2D>().isKinematic = false;
            GetComponentInChildren<ParticleSystem>().Stop();
            StopAllCoroutines();
            Invoke("End", 1.5f);
        }

        private void End() => Destroy(this.gameObject);

        private void Update() 
        {
            if (m_On && !m_MoveLock)
                if (m_MoveToTarget)
                    StartCoroutine(MoveTowards(m_Target));
                else
                    StartCoroutine(MoveTowards(m_Origin));
        }

        private IEnumerator MoveTowards(float y)
        {
            m_MoveLock = true;

            if (m_Body.position.y > y)
                while (m_Body.position.y > y)
                {
                    m_Body.Translate
                    (
                        x: 0f,
                        y: -k_SPEED * Time.deltaTime,
                        z: 0f
                    );
                    yield return null;
                }
            else while (m_Body.position.y < y)
                {
                    m_Body.Translate
                    (
                        x: 0f,
                        y: k_SPEED * Time.deltaTime,
                        z: 0f
                    );
                    yield return null;
                }

            m_MoveLock = false;
            m_MoveToTarget = !m_MoveToTarget;

            yield return 0;
        }

        private void OnDrawGizmos() 
        {
            var body = GetComponent<Transform>();
            Debug.DrawLine
            (
                start: new Vector3
                (
                    x: body.position.x - 0.5f,
                    y: body.position.y,
                    z: body.position.z
                ),
                end: new Vector3
                (
                    x: body.position.x - 0.5f,
                    y: body.position.y + k_UP_DISTANCE,
                    z: body.position.z
                ),
                color: Color.magenta,
                duration: 0f
            );    
        }
    }
}
